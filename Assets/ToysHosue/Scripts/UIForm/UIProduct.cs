using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System.IO;
using UnityEngine.SceneManagement;

namespace muzi
{
    public class UIProduct : MonoBehaviour
    {
        public int ID { get; set; }

        [SerializeField]
        private Text _txtName;
        [SerializeField]
        private RawImage _imgIcon;
        [SerializeField]
        private Text _txtDescription;
        [SerializeField]
        private Text _txtDownloadCount;
        [SerializeField]
        private Button _btnDownloadState;

        private string _fileUrl;
        private string _addCownloadCountUrl;
        private Text _txtState;
        private Image _imgProgress;
        private float _downloadProgress;

        private string _savePath;
        private string _scenePath = "/scene/modelshow.dat";
        private bool _unzipOver = false;

        private DownloadState _downloadState = DownloadState.NotDownloaded;

        public DownloadState DownLoadState
        {
            get { return _downloadState; }
            set
            {
                if (_downloadState != value)
                {
                    _downloadState = value;
                    OnStateChange();
                }
            }
        }

        void Awake()
        {
            _addCownloadCountUrl = MainUIProcedure.Domain + "/api/arproduct/addDownloadCount?id=";

            GetComponent<LayoutElement>().preferredWidth = Screen.width;
            GetComponent<LayoutElement>().preferredHeight = 256;

            _txtState = _btnDownloadState.transform.Find("TxtState").GetComponent<Text>();
            _imgProgress = _btnDownloadState.transform.Find("ImgProgress").GetComponent<Image>();

            _imgProgress.fillAmount = 0f;
            _downloadProgress = 0f;
        }

        private void OnEnable()
        {
            _btnDownloadState.onClick.AddListener(OnChangeStateClick);
            //TODO:监听获取下载状态事件
            EntryInstance.Event.Subscribe(DownloadSuccessEventArgs.EventId, OnDownloadSuccess);
            EntryInstance.Event.Subscribe(DownloadFailureEventArgs.EventId, OnDownloadFailure);
            //TODO:监听下载进度事件
        }

        private void OnDisable()
        {
            _btnDownloadState.onClick.RemoveListener(OnChangeStateClick);

            EntryInstance.Event.Unsubscribe(DownloadSuccessEventArgs.EventId, OnDownloadSuccess);
            EntryInstance.Event.Unsubscribe(DownloadFailureEventArgs.EventId, OnDownloadFailure);
        }

        private void OnDownloadSuccess(object sender, GameEventArgs e)
        {
            DownloadSuccessEventArgs args = (DownloadSuccessEventArgs)e;
            if (args.UserData != this)
            {
                return;
            }
            Debug.Log(_txtName.text + " 下载完成");
            DownLoadState = DownloadState.Download;
            //解压文件
            ZipUtility.AsyncUnzipFile(args.DownloadPath, _savePath, null, new UnzipCallback(args.DownloadPath, OnUnzipSucceed));
            StopCoroutine("CheckUnzipOver");
            StartCoroutine("CheckUnzipOver");
            DownLoadState = DownloadState.Unziping;
        }

        void OnUnzipSucceed(bool isSucceed)
        {
            _downloadState = DownloadState.Compete;
        }

        IEnumerator CheckUnzipOver()
        {
            float outTime = 90f;
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                if (DownLoadState == DownloadState.Compete)
                {
                    OnStateChange();
                    break;
                }

                if (outTime <= 0f)
                {
                    Debug.Log("解压超时");
                    break;
                }
                outTime -= 0.1f;
            }
        }

        private void OnDownloadFailure(object sender, GameEventArgs e)
        {
            DownloadSuccessEventArgs args = (DownloadSuccessEventArgs)e;
            if (args.UserData != this)
            {
                return;
            }
            Debug.Log("下载失败");
        }

        public void Binding(string title, string iconurl, string description, string fileurl, int downloadcount)
        {
            _txtName.text = title;
            _txtDescription.text = description;
            _fileUrl = fileurl;
            _txtDownloadCount.text = downloadcount.ToString();
            if (_imgIcon.texture == null)
                StartCoroutine(GetIconTexture(iconurl));

            _savePath = string.Format("{0}/products/{1}/", Application.persistentDataPath,ID);
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            if (File.Exists(string.Format("{0}{1}{2}", _savePath, _txtName.text, _scenePath)))
            {
                DownLoadState = DownloadState.Compete;
            }
        }

        /// <summary>
        /// 获取Icon图标
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IEnumerator GetIconTexture(string url)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    _imgIcon.texture = DownloadHandlerTexture.GetContent(uwr);
                }
            }
        }

        /// <summary>
        /// 清空当前列表数据
        /// </summary>
        public void Recover()
        {
            _txtName.text = null;
            _imgIcon.texture = null;
            _txtDescription.text = null;
            _txtDownloadCount.text = null;

            _txtState.text = "下载";
            _imgProgress.fillAmount = 0f;
            _downloadProgress = 0f;
            DownLoadState = DownloadState.NotDownloaded;
        }

        private void OnChangeStateClick()
        {
            switch (DownLoadState)
            {
                case DownloadState.NotDownloaded:
                    //TODO:验证最大下载文件数量,然后下载
                    DownLoadState = DownloadState.Downloading;
                    break;
                case DownloadState.Downloading:
                    //DownLoadState = DownloadState.Pause;
                    break;
                case DownloadState.Download:
                    EntryInstance.WebRequest.AddWebRequest(_addCownloadCountUrl + ID);
                    break;
                case DownloadState.Pause:
                    //DownLoadState = DownloadState.Downloading;
                    break;
                case DownloadState.Compete:
                    //TODO:校验文件，进入场景
                    if (File.Exists(string.Format("{0}{1}{2}", _savePath, _txtName.text, _scenePath)))
                    {
                        EntryInstance.Resource.ResourceHelper.LoadAssetByWWW(string.Format("file://{0}{1}{2}", _savePath, _txtName.text, _scenePath), OnLoadAssetCompete);
                    }
                    else
                    {
                        EntryInstance.UI.OpenDialog(new DialogParams()
                        {
                            Mode = 1,
                            Title = "提示",
                            Message = "文件不存在，请重新下载！",
                            ConfirmText = "确定",
                            OnClickConfirm = (data) => { DownLoadState = DownloadState.NotDownloaded; }
                        });
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnLoadAssetCompete(AssetBundle assetBundle, string errorMessage)
        {
            if (assetBundle != null)
            {
                //EntryInstance.Scene.UnloadScene(AssetUtility.GetSceneAsset("Scan"));
                SceneManager.LoadScene("ModelShow", LoadSceneMode.Additive);
                EntryInstance.UI.CloseUIForm(EntryInstance.UI.GetUIForm(UIFormId.UIBottomBarForm, UIGroup.BottomBar.ToString()));
                EntryInstance.UI.OpenUIForm(AssetUtility.GetUIFormAsset(UIFormId.UIARForm), UIGroup.Content.ToString(), true, UIFormId.UIARForm);
                assetBundle.Unload(false);
            }
            if (errorMessage != null)
            {
                Debug.Log(errorMessage);
            }
        }

        private void OnStateChange()
        {
            //TODO:给下载组件发消息
            switch (DownLoadState)
            {
                case DownloadState.NotDownloaded:
                    _txtState.text = "下载";
                    _imgProgress.fillAmount = 0f;
                    _downloadProgress = 0f;
                    break;
                case DownloadState.Downloading:
                    //TODO:发送(继续)下载文件消息
                    DownLoadFile();
                    _txtState.text = "下载中";
                    break;
                case DownloadState.Download:
                    _txtState.text = "下载完成";
                    break;
                case DownloadState.Pause:
                    //TODO:发送暂停下载文件消息
                    _txtState.text = "暂停";
                    break;
                case DownloadState.Unziping:
                    _txtState.text = "解压中";
                    break;
                case DownloadState.Compete:
                    _txtState.text = "进入";
                    break;
                default:
                    break;
            }
        }

        private void DownLoadFile()
        {
            EntryInstance.Download.AddDownload(_savePath  + _txtName.text + ".zip", _fileUrl, this);
        }

        IEnumerator TestDownload()
        {
            _downloadProgress = _imgProgress.fillAmount;
            while (DownLoadState == DownloadState.Downloading)
            {
                _downloadProgress += Time.deltaTime * 0.01f;
                if (_downloadProgress >= 1)
                {
                    _imgProgress.fillAmount = 1;
                    DownLoadState = DownloadState.Download;
                    yield break;
                }
                _imgProgress.fillAmount = _downloadProgress;
            }
        }
    }
}
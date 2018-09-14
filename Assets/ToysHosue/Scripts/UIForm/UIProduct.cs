using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System.IO;

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
        private Text _txtState;
        private Image _imgProgress;
        private float _downloadProgress;

        private string _savePath;
        private string _downloadPath;

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

        void Start()
        {

            GetComponent<LayoutElement>().preferredWidth = Screen.width;
            GetComponent<LayoutElement>().preferredHeight = 256;

            _txtState = _btnDownloadState.transform.Find("TxtState").GetComponent<Text>();
            _imgProgress = _btnDownloadState.transform.Find("ImgProgress").GetComponent<Image>();

            _imgProgress.fillAmount = 0f;
            _downloadProgress = 0f;
            _savePath = Application.persistentDataPath+"/products";
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
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
            if (args.UserData!=this)
            {
                return;
            }
            Debug.Log(_txtName.text + " 下载完成");
            //解压文件
            ZipUtility.AsyncUnzipFile(args.DownloadPath, _savePath, null, new UnzipCallback(args.DownloadPath));
            _txtState.text = "进入";
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

        public void Binding(string title, string iconurl, string description,string fileurl, int downloadcount)
        {
            _txtName.text = title;
            _txtDescription.text = description;
            _fileUrl = fileurl;
            _txtDownloadCount.text = downloadcount.ToString();
            StartCoroutine(GetText(iconurl));
        }

        /// <summary>
        /// 获取Icon图标
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IEnumerator GetText(string url)
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
                    DownLoadState = DownloadState.Pause;
                    break;
                case DownloadState.Download:
                    //TODO:校验文件，进入场景
                    break;
                case DownloadState.Pause:
                    DownLoadState = DownloadState.Downloading;
                    break;
                default:
                    break;
            }
        }

        private void OnStateChange()
        {
            //TODO:给下载组件发消息
            switch (DownLoadState)
            {
                case DownloadState.NotDownloaded:
                    break;
                case DownloadState.Downloading:
                    //TODO:发送(继续)下载文件消息
                    DownLoadFile();
                    _txtState.text = "下载中";
                    break;
                case DownloadState.Download:
                    _txtState.text = "进入";
                    break;
                case DownloadState.Pause:
                    //TODO:发送暂停下载文件消息
                    _txtState.text = "暂停";
                    break;
                default:
                    break;
            }
        }

        private void DownLoadFile()
        {
            EntryInstance.Download.AddDownload(_savePath + "/"+_txtName.text+ ".zip", _fileUrl, this);
        }

        IEnumerator TestDownload()
        {
            _downloadProgress = _imgProgress.fillAmount;
            while (DownLoadState==DownloadState.Downloading)
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
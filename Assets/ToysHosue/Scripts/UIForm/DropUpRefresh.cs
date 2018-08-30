using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace muzi
{
    public class DropUpRefresh : MonoBehaviour
    {

        [SerializeField]
        private ScrollRect _productScrollView;
        [SerializeField]
        private Image _imgProgress;
        private float _progress;
        private float _dragSize = 200f;
        void Start()
        {
            //GetComponent<LayoutElement>().preferredWidth = Screen.width;
        }

        private void OnEnable()
        {
            _productScrollView.onValueChanged.AddListener(OnScrollValueChangeed);
            _progress = 0f;
        }

        private void OnDisable()
        {
            _productScrollView.onValueChanged.RemoveListener(OnScrollValueChangeed);
        }

        public void OnScrollValueChangeed(Vector2 pos)
        {
            transform.localPosition = _productScrollView.content.localPosition - Vector3.up * _productScrollView.content.sizeDelta.y;
            _progress = _productScrollView.viewport.rect.height - Mathf.Abs(transform.localPosition.y);
            if (_progress > 0)
            {
                //_progress = Mathf.Clamp01(Mathf.Round(_progress / _dragSize * 100) / 100f);
                _imgProgress.fillAmount = Mathf.Clamp01(_progress / _dragSize);
                if (_imgProgress.fillAmount == 1)
                {
                    Debug.Log("刷新列表");
                    //TODO:请求数据，刷新列表
                    StartCoroutine(TestRefresh());
                }
            }
        }

        IEnumerator TestRefresh()
        {
            Debug.Log("TestRefresh");
            _productScrollView.enabled = false;
            AssetsRequestProcedure assetRequsetProcedure = EntryInstance.Procedure.GetProcedure<AssetsRequestProcedure>() as AssetsRequestProcedure;
            assetRequsetProcedure.RequestAsset();
            yield return new WaitForSeconds(2f);
            _productScrollView.enabled = true;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace muzi
{
    public class UIHeadImage : MonoBehaviour
    {
        [SerializeField]
        private RawImage _headImage;

        private void Start()
        {
            //GetComponent<LayoutElement>().preferredWidth = Screen.width;
        }

        public void Binding(Texture2D headimage)
        {
            _headImage.texture = headimage;
        }
    }
}
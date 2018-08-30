using GameFramework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;

namespace muzi
{
    public class UIGroupHelper : UIGroupHelperBase
    {
        private Canvas _groupCanvas;
        private const int DepthFactor = 100;
        private int _depth = 0;

        private void Awake()
        {
            _groupCanvas = gameObject.GetOrAddComponent<Canvas>();
            gameObject.GetOrAddComponent<GraphicRaycaster>();
        }

        void Start()
        {
            _groupCanvas.overrideSorting = true;
            _groupCanvas.sortingOrder = _depth * DepthFactor;
            RectTransform trans = GetComponent<RectTransform>();
            trans.anchorMin = Vector2.zero;
            trans.anchorMax = Vector2.one;
            trans.anchoredPosition = Vector2.zero;
            trans.sizeDelta = Vector2.zero;
        }

        public override void SetDepth(int depth)
        {
            _depth = depth;
            _groupCanvas.overrideSorting = true;
            _groupCanvas.sortingOrder = depth * DepthFactor;
        }
    }
}

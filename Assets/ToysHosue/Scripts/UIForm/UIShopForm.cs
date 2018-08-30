using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace muzi
{
    public class UIShopForm : UIFormLogic
    {
        [SerializeField]
        private UIHeadImage _uiHeadImagePre;
        [SerializeField]
        private UIProduct _uiProductPre;
        [SerializeField]
        private ScrollRect _headScrollView;
        [SerializeField]
        private ScrollRect _productScrollView;

        private List<UIProduct> _productList;
        private List<UIHeadImage> _headImage;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            GetComponent<RectTransform>().offsetMin = Vector2.zero;
            GetComponent<RectTransform>().offsetMax = Vector2.zero;

            _productList = new List<UIProduct>();
            _headImage = new List<UIHeadImage>();

            //CreateProductItems();
            CreateHeadImgItems();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            EntryInstance.Event.Subscribe((int)EventId.ReceiveProductData, OnReceiveProductData);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            EntryInstance.Event.Unsubscribe((int)EventId.ReceiveProductData, OnReceiveProductData);
        }

        private void OnReceiveProductData(object sender, GameEventArgs e)
        {
            ReceiveProductDataEventArgs args = (ReceiveProductDataEventArgs)e;
            for (int i = 0; i < args.ProductModels.Length; i++)
            {
                UIProduct product = GetProduct();
                if (product!=null)
                {
                    ProductModel productModel = args.ProductModels[i];
                    product.Binding(productModel.name, AssetsRequestProcedure.Domain + productModel.iconimage, productModel.description, AssetsRequestProcedure.Domain+productModel.assetfile, productModel.downloadcount);
                }
            }
        }

        //private void CreateProductItems()
        //{
        //    for (int i = 0; i < 12; i++)
        //    {
        //        UIProduct product = Instantiate<UIProduct>(_uiProductPre, _productScrollView.content);
        //        _productList.Add(product);
        //        product.gameObject.SetActive(false);
        //    }
        //}

        private void CreateHeadImgItems()
        {
            for (int i = 0; i < 5; i++)
            {
                UIHeadImage img = Instantiate<UIHeadImage>(_uiHeadImagePre, _headScrollView.content);
                _headImage.Add(img);
            }
        }

        private UIProduct GetProduct()
        {
            foreach (var product in _productList)
            {
                if (!product.gameObject.activeSelf)
                {
                    product.gameObject.SetActive(true);
                    return product;
                }
            }
            UIProduct uiProduct = Instantiate<UIProduct>(_uiProductPre, _productScrollView.content);
            _productList.Add(uiProduct);
            return uiProduct;
        }
    }
}

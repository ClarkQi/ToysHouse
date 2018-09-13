using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameFramework.Resource;
namespace muzi
{
    public class UIShopForm : UGuiForm
    {
        [SerializeField]
        private UIProduct _uiProductPre;
        [SerializeField]
        private ScrollRect _productScrollView;

        private List<UIProduct> _productList;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            formId = UIFormId.UIShopForm;

            Screen.orientation = ScreenOrientation.Portrait;

            _productScrollView = transform.Find("ProductScrollView").GetComponent<ScrollRect>();

            _productList = new List<UIProduct>();

            MainUIProcedure mainUIProcedure = EntryInstance.Procedure.GetProcedure<MainUIProcedure>() as MainUIProcedure;
            mainUIProcedure.RequestAsset();
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
                UIProduct product = GetProduct(args.ProductModels[i].id);
                if (product!=null)
                {
                    ProductModel productModel = args.ProductModels[i];
                    product.ID = productModel.id;
                    product.Binding(productModel.name, MainUIProcedure.Domain + productModel.iconimage, productModel.description, MainUIProcedure.Domain+productModel.assetfile, productModel.downloadcount);
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

        private UIProduct GetProduct(int id)
        {
            foreach (var product in _productList)
            {
                if (product.ID==id)
                {
                    return product;
                }
            }
            UIProduct uiProduct = Instantiate<UIProduct>(_uiProductPre, _productScrollView.content);
            _productList.Add(uiProduct);
            return uiProduct;
        }

        //private void LoadAssetSucceed(string assetName, object asset, float duration, object userData)
        //{
        //    if (userData!=this)
        //    {
        //        return;
        //    }
        //    Debug.Log("LoadAssetSucceed=" + assetName);
        //}

        //private void LoadAssetFailed(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        //{
        //    if (userData != this)
        //    {
        //        return;
        //    }
        //    Debug.Log(assetName);
        //}
    }
}

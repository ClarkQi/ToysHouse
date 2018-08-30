using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using System;

namespace muzi
{
    public class ReceiveProductDataEventArgs : GameEventArgs
    {
        public ProductModel[] ProductModels { get; set; }
        public override int Id
        {
            get
            {
                return (int)EventId.ReceiveProductData;
            }
        }

        public override void Clear()
        {
            ProductModels = null;
        }
    }
}

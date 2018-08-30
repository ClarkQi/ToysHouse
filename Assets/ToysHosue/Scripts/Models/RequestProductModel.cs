using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace muzi
{
    //[System.Serializable]
    public class RequestProductModel
    {

        public int code { get; set; }
        public string msg { get; set; }
        public string time { get; set; }
        public ProductModel[] data { get; set; }
    }
}
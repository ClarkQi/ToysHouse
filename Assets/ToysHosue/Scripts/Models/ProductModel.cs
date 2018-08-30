using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace muzi
{
    //[System.Serializable]
    public class ProductModel
    {

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string iconimage { get; set; }
        public string assetfile { get; set; }
        public int version { get; set; }
        public int downloadcount { get; set; }
    }
}

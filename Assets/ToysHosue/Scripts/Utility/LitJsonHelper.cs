using GameFramework;
using LitJson;
using System;

namespace muzi
{
    public class LitJsonHelper : Utility.Json.IJsonHelper
    {
        public string ToJson(object obj)
        {
            return JsonMapper.ToJson(obj);
        }

        public object ToObject(Type objectType, string json)
        {
            throw new NotImplementedException("使用ToObject<T>代替");
        }

        public T ToObject<T>(string json)
        {
            return JsonMapper.ToObject<T>(json);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using System.IO;
using System;

namespace muzi
{
    public class UnzipCallback : ZipUtility.UnzipCallback
    {
        string _path=null;
        Action<bool> _callBack;
        public UnzipCallback(string filePath)
        {
            _path = filePath;
        }

        public UnzipCallback(string filePath, Action<bool> callback)
        {
            _path = filePath;
            _callBack = callback;
        }
        /// <summary>
        /// 解压单个文件或文件夹前执行的回调
        /// </summary>
        /// <param name="_entry"></param>
        /// <returns>如果返回true，则解压缩文件，反之则不解压缩文件</returns>
        public override bool OnPreUnzip(ZipEntry _entry)
        {
            //TODO:检查当前有几个文件在解压(如果太多文件同时解压占用内存过大)，等待解压。
            return true;
        }
        /// <summary>
        /// 解压单个文件或文件夹后执行的回调
        /// </summary>
        /// <param name="_entry"></param>
        public override void OnPostUnzip(ZipEntry _entry)
        {
            
        }
        /// <summary>
        /// 解压执行完毕后的回调
        /// </summary>
        /// <param name="_result">true表示解压成功，false表示解压失败</param>
        public override void OnFinished(bool _result)
        {
            if (_result)
            {
                Debug.Log("解压成功");
                try
                {
                    File.Delete(_path);
                    Debug.Log("删除文件成功");
                }
                catch (System.Exception)
                {
                    throw new System.Exception("删除文件出错");
                }
                if (_callBack != null)
                {
                    _callBack(true);
                }
            }
            else
            {
                if (_callBack != null)
                {
                    _callBack(false);
                }
            }
        }
    }
}
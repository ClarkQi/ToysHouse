using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Resource;

namespace muzi
{
    public sealed class LoadResourceErrorEventArgs : GameFrameworkEventArgs
    {
        //
        // 摘要:
        //     /// 初始化加载资源代理辅助器错误事件的新实例。 ///
        //
        // 参数:
        //   status:
        //     加载资源状态。
        //
        //   errorMessage:
        //     错误信息。
        public LoadResourceErrorEventArgs(LoadResourceStatus status, string errorMessage)
        {
            ErrorMessage = errorMessage;
            Status = status;
        }

        //
        // 摘要:
        //     /// 获取错误信息。 ///
        public string ErrorMessage { get; private set; }
        //
        // 摘要:
        //     /// 获取加载资源状态。 ///
        public LoadResourceStatus Status { get; private set; }
    }
}
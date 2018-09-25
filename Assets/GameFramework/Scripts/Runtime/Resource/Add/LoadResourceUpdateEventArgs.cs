using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Resource;

namespace muzi
{
    public class LoadResourceUpdateEventArgs : GameFrameworkEventArgs
    {
        //
        // 摘要:
        //     /// 初始化加载资源代理辅助器更新事件的新实例。 ///
        //
        // 参数:
        //   type:
        //     进度类型。
        //
        //   progress:
        //     进度。
        public LoadResourceUpdateEventArgs(LoadResourceProgress type, float progress)
        {
            Progress = progress;
            Type = type;
        }

        //
        // 摘要:
        //     /// 获取进度。 ///
        public float Progress { get; private set; }
        //
        // 摘要:
        //     /// 获取进度类型。 ///
        public LoadResourceProgress Type { get; private set; }
    }
}
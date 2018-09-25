using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace muzi
{
    public class LoadResourceCompentEventArgs : GameFrameworkEventArgs
    {
        public LoadResourceCompentEventArgs(object resource)
        {
            Resource = resource;
        }
        public object Resource { get;private set; }
    }
}
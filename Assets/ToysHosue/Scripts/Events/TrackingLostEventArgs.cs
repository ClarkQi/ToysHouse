using System;
using GameFramework.Event;
using UnityEngine;
using Vuforia;

namespace muzi
{
    public class TrackingLostEventArgs : GameEventArgs
    {
        public GameObject Tracker { get; private set; }
        public TrackableBehaviour TrackableBehaviour { get; private set; }

        public override int Id
        {
            get
            {
                return (int)EventId.TrackingLost;
            }
        }

        public TrackingLostEventArgs(GameObject tracker, TrackableBehaviour trackableBehaviour)
        {
            Tracker = tracker;
            TrackableBehaviour = trackableBehaviour;
        }

        public override void Clear()
        {
            Tracker = null;
            TrackableBehaviour = null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using System;
using Vuforia;

namespace muzi
{
    public class TrackingFoundEventArgs :GameEventArgs
    {
        public GameObject Tracker { get; private set; }
        public TrackableBehaviour TrackableBehaviour { get; private set; }

        public override int Id
        {
            get
            {
                return (int)EventId.TrackingFound;
            }
        }

        public TrackingFoundEventArgs(GameObject tracker, TrackableBehaviour trackableBehaviour)
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
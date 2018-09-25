using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace muzi
{
    public class TrackManager : MonoBehaviour
    {
        bool _isTracking = false;

        void Start()
        {
            EntryInstance.Event.Subscribe((int)EventId.TrackingFound, OnTrackingFound);
            EntryInstance.Event.Subscribe((int)EventId.TrackingLost, OnTrackingLost);
        }

        private void OnDestroy()
        {
            EntryInstance.Event.Unsubscribe((int)EventId.TrackingFound, OnTrackingFound);
            EntryInstance.Event.Unsubscribe((int)EventId.TrackingLost, OnTrackingLost);
        }

        private void OnTrackingFound(object sender, GameEventArgs e)
        {
            _isTracking = true;

            Debug.Log("OnTrackingFound");
        }
        private void OnTrackingLost(object sender, GameEventArgs e)
        {
            _isTracking = false;
            Debug.Log("OnTrackingLost");
        }
    }
}
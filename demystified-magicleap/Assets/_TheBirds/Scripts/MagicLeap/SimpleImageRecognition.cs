// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

namespace MagicLeap
{
    /// <summary>
    /// This class handles visibility on image tracking, displaying and hiding prefabs
    /// when images are detected or lost.
    /// </summary>
    [RequireComponent(typeof(MLImageTrackerBehavior))]
    public class SimpleImageRecognition : MonoBehaviour
    {
        #region Private Variables
        private MLImageTrackerBehavior _trackerBehavior;
        [HideInInspector]
        public bool _targetFound = false;

        [HideInInspector]
        public bool firstDetectionMade = false; 
        [SerializeField, Tooltip("Text to update on ImageTracking changes.")]
       // private Text _statusLabel;
        // Stores initial text
      //  private string _prefix;

       // [SerializeField, Tooltip("Game Object showing the axis")]
        [HideInInspector]
        private GameObject _axis;
        [SerializeField, Tooltip("Game Object showing the tracking cube")]
        [HideInInspector]
        private GameObject _trackingCube;
        [SerializeField, Tooltip("Game Object showing the demo")]
        private GameObject _ImageAnchor;
        #endregion

        #region Unity Methods
       
        /// <summary>
        /// Initializes variables and register callbacks
        /// </summary>
        void Start()
        {
            _trackerBehavior = GetComponent<MLImageTrackerBehavior>();
            _trackerBehavior.OnTargetFound += OnTargetFound;
            _trackerBehavior.OnTargetLost += OnTargetLost;

            RefreshViewMode();
        }

        /// <summary>
        /// Unregister calbacks
        /// </summary>
        void OnDestroy()
        {
            _trackerBehavior.OnTargetFound -= OnTargetFound;
            _trackerBehavior.OnTargetLost -= OnTargetLost;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Update which objects should be visible
        /// </summary>
        /// <param name="viewMode">Contains the mode to view</param>
        public void UpdateViewMode(ImageTrackingExample.ViewMode viewMode)
        {
            RefreshViewMode();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// De/Activate objects to be hidden/seen
        /// </summary>
        private void RefreshViewMode()
        {

            if(!firstDetectionMade && _targetFound){
                firstDetectionMade = true;
                _ImageAnchor.SetActive(true);

            }

           
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Callback for when tracked image is found
        /// </summary>
        /// <param name="isReliable"> Contains if image found is reliable </param>
        private void OnTargetFound(bool isReliable)
        {
           // _statusLabel.text = String.Format("{0}Target Found ({1})", _prefix, (isReliable ? "Reliable" : "Unreliable"));
            _targetFound = true;
            RefreshViewMode();
        }

        /// <summary>
        /// Callback for when image tracked is lost
        /// </summary>
        private void OnTargetLost()
        {
           // _statusLabel.text = String.Format("{0}Target Lost", _prefix);
            _targetFound = false;
            RefreshViewMode();
        }
        #endregion
    }
}

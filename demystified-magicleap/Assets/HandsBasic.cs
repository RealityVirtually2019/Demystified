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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace MagicLeap
{
    /// <summary>
    /// Component used to hook into the Hand Tracking script and attach
    /// primitive game objects to it's detected keypoint positions for
    /// each hand.
    /// </summary>
    public class HandsBasic : MonoBehaviour
    {
        #region Private Variables
        [SerializeField, Tooltip("The hand to visualize.")]
        private MLHandType _handType;

        [SerializeField, Tooltip("The GameObject to use for the Hand Center.")]
        private Transform _center;

       
        #endregion

        #region Private Properties
        /// <summary>
        /// Returns the hand based on the hand type.
        /// </summary>
        private MLHand Hand
        {
            get
            {
                if (_handType == MLHandType.Left)
                {
                    return MLHands.Left;
                }
                else
                {
                    return MLHands.Right;
                }
            }
        }
        #endregion

        #region Unity Methods
        /// <summary>
        /// Initializes MLHands API.
        /// </summary>
        void Start()
        {
            MLResult result = MLHands.Start();
            if (!result.IsOk)
            {
                Debug.LogErrorFormat("Error: HandTrackingVisualizer failed starting MLHands, disabling script. Reason: {0}", result);
                enabled = false;
                return;
            }
        }

        /// <summary>
        /// Stops the communication to the MLHands API.
        /// </summary>
        void OnDestroy()
        {
            if (MLHands.IsStarted)
            {
                MLHands.Stop();
            }
        }

        /// <summary>
        /// Update the keypoint positions.
        /// </summary>
        void Update()
        {
            if (MLHands.IsStarted)
            {
              

                // Hand Center
                if (_center != null)
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, Hand.Center, Time.deltaTime *10f);
                   
                }
            }
        }
        #endregion

        #region Private Methods
      
        /*
        /// <summary>
        /// Create a GameObject for the desired KeyPoint.
        /// </summary>
        /// <param name="keyPoint"></param>
        /// <returns></returns>
        private GameObject CreateKeyPoint(MLKeyPoint keyPoint, Color color)
        {
            GameObject newObject;

            newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newObject.transform.SetParent(transform);
            newObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            newObject.name = keyPoint.ToString();
            newObject.GetComponent<Renderer>().material.color = color;

            return newObject;
        }
        */

        #endregion
    }
}

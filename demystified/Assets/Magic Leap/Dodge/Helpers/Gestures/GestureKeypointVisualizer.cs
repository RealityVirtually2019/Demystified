// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine.XR.MagicLeap;
using UnityEngine;

namespace MagicKit
{

    ///<summary>
    /// Places a copy of the prefab on all active keypoints during a gesture.
    ///</summary>
    public class GestureKeypointVisualizer : MonoBehaviour 
    {

        //----------- Private Members -----------

        [SerializeField, Tooltip("Object displayed at each point.")]
        private GameObject _keypointPrefab;
        [SerializeField, Tooltip("Object displayed in the center of the hand.")]
        private GameObject _centerPrefab;
        [SerializeField,BitMask(typeof(KeyPoseTypes)), Tooltip("These keyposes will be visualized on the left hand.")]
        private KeyPoseTypes _leftTypes;
        [SerializeField, BitMask(typeof(KeyPoseTypes)), Tooltip("These keyposes will be visualized on the left hand.")]
        private KeyPoseTypes _rightTypes;
        
        private List<GameObject> _leftIndicators;
        private List<GameObject> _rightIndicators;

        //----------- MonoBehaviour Methods -----------

        private void OnEnable()
        {
            GestureManager.OnGestureBegin += OnGestureBegin;
            GestureManager.OnGestureEnd += OnGestureEnd;
            GestureManager.OnGestureStay += OnGestureStay;
        }

        private void OnDisable()
        {
            GestureManager.OnGestureBegin -= OnGestureBegin;
            GestureManager.OnGestureEnd -= OnGestureEnd;
            GestureManager.OnGestureStay -= OnGestureStay;
        }

        //----------- Event Handlers -----------

        private void OnGestureBegin(MLHand hand)
        {
            if (GestureManager.Matches(hand, _leftTypes, _rightTypes))
            {
                EnableContent(hand, true);
            }
        }

        private void OnGestureStay(MLHand hand)
        {
            if(GestureManager.Matches(hand,_leftTypes,_rightTypes))
            {
                DrawKeypoints(hand);
            }
           
        }

        private void OnGestureEnd(MLHandType handType, MLHandKeyPose keyPose)
        {
            MLHand hand = handType == MLHandType.Left ? MLHands.Left : MLHands.Right;
            EnableContent(hand, false);
        }

        //----------- Private Methods -----------

        private void EnableContent(MLHand hand,bool enabled)
        {
            List<GameObject> indicators = GetIndicators(hand);
            foreach (GameObject indicator in indicators)
            {
                indicator.SetActive(enabled);
            }
        }

        private void DrawKeypoints(MLHand hand)
        {
            //position keypoint indicators
            List<MLKeyPoint> keypoints = GetKeypoints(hand);
            List<GameObject> indicators = GetIndicators(hand);
            for (int i = 0; i < keypoints.Count; i++)
            {
                if (keypoints[i].IsValid)
                {
                    indicators[i].SetActive(true);
                    indicators[i].transform.position = keypoints[i].Position;
                }
                else
                {
                    indicators[i].SetActive(false);
                }
            }
            // position the hand.center indicator
            indicators[indicators.Count - 1].transform.position = hand.Center;
        }

        private List<GameObject> GetIndicators(MLHand hand)
        {
            if(hand.HandType == MLHandType.Left)
            {
                if(_leftIndicators == null)
                {
                    _leftIndicators = new List<GameObject>();
                    List<MLKeyPoint> keypoints = GetKeypoints(hand);
                    for(int i = 0; i < keypoints.Count; i++)
                    {
                        GameObject newIndicator = Instantiate(_keypointPrefab);
                        _leftIndicators.Add(newIndicator);
                        newIndicator.SetActive(false);
                    }
                    //add centerpoint as final indicator in the list.
                    GameObject leftCenter = Instantiate(_centerPrefab);
                    leftCenter.SetActive(false);
                    _leftIndicators.Add(leftCenter);
                }

                return _leftIndicators;
            }
            else
            {
                if (_rightIndicators == null)
                {
                    _rightIndicators = new List<GameObject>();
                    List<MLKeyPoint> keypoints = GetKeypoints(hand);
                    for (int i = 0; i < keypoints.Count; i++)
                    {
                        GameObject newIndicator = Instantiate(_keypointPrefab);
                        _rightIndicators.Add(newIndicator);
                        newIndicator.SetActive(false);
                    }
                    //add centerpoint as final indicator in the list.
                    GameObject rightCenter = Instantiate(_centerPrefab);
                    rightCenter.SetActive(false);
                    _rightIndicators.Add(rightCenter);
                }

                return _rightIndicators;
            }
        }

        private List<MLKeyPoint> GetKeypoints(MLHand hand)
        {
            List<MLKeyPoint> keypoints = new List<MLKeyPoint>();
            keypoints.AddRange(hand.Wrist.KeyPoints);
            keypoints.AddRange(hand.Thumb.KeyPoints);
            keypoints.AddRange(hand.Index.KeyPoints);
            keypoints.AddRange(hand.Middle.KeyPoints);
            keypoints.AddRange(hand.Ring.KeyPoints);
            keypoints.AddRange(hand.Pinky.KeyPoints);
            return keypoints;
        }
    }
}

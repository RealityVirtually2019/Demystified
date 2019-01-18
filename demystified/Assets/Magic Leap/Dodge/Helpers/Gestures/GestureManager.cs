// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using MagicLeap.Utilities;
using System;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace MagicKit
{

    ///<summary>
    /// One stop shop for gestures.
    ///</summary>
    public class GestureManager : Singleton<GestureManager>
    {

        //----------- Public Events -----------

        public static event Action<MLHand> OnGestureBegin;
        public static event Action<MLHandType, MLHandKeyPose> OnGestureEnd;
        public static event Action<MLHand> OnGestureStay;

        //----------- Public Members -----------

        public static MLHand LeftHand
        {
            get { return MLHands.Left; }
        }

        public static MLHand RightHand
        {
            get { return MLHands.Right; }
        }

        //----------- Private Members -----------

        [SerializeField, Tooltip("Gestures must be > this confidence to be considered valid."), Range(0, 1)]
        private float _startConfidence = 0.9f;
        [SerializeField, Tooltip("If a gesture drops below this confidence it will be stopped."), Range(0, 1)]
        private float _stopConfidence = 0.5f;

        private bool _leftRecognized;
        private bool _rightRecognized;
        private bool _leftActive;
        private bool _rightActive;
        private MLHandKeyPose _lastLeftKeyPose;
        private MLHandKeyPose _lastRightKeyPose;

        //----------- MonoBehaviour Methods -----------

        private void OnEnable()
        {
            MLResult result = MLHands.Start();
            if (!result.IsOk)
            {
                Debug.LogError("MLHands.Start() error");
                enabled = false;
                return;
            }

            MLHands.KeyPoseManager.OnHandKeyPoseBegin += OnHandKeyPoseBegin;
            MLHands.KeyPoseManager.OnHandKeyPoseEnd += OnHandKeyPoseEnd;
        }

        private void OnDisable()
        {
            MLHands.KeyPoseManager.OnHandKeyPoseBegin -= OnHandKeyPoseBegin;
            MLHands.KeyPoseManager.OnHandKeyPoseEnd -= OnHandKeyPoseEnd;
            MLHands.Stop();
        }

        private void Update()
        {

            //OnGestureEnd is called when (hand was active AND (hand is no longer recognized OR keyPose confidence value dropped OR keypose changed))
            if (_leftActive && (!_leftRecognized || MLHands.Left.KeyPoseConfidenceFiltered < _stopConfidence || MLHands.Left.KeyPose != _lastLeftKeyPose))
            {
                _leftActive = false;
                if (OnGestureEnd != null)
                {
                    OnGestureEnd(MLHandType.Left, _lastLeftKeyPose);
                }
            }
            if (_rightActive && (!_rightRecognized || MLHands.Right.KeyPoseConfidenceFiltered < _stopConfidence || MLHands.Right.KeyPose != _lastRightKeyPose))
            {
                _rightActive = false;
                if (OnGestureEnd != null)
                {
                    OnGestureEnd(MLHandType.Right,_lastRightKeyPose);
                }
            }

            //OnGestureBegin
            if (_leftRecognized && !_leftActive)
            {
                if (MLHands.Left.KeyPoseConfidence > _startConfidence)
                {
                    _leftActive = true;
                    if (OnGestureBegin != null)
                    {
                        OnGestureBegin(MLHands.Left);
                    }
                }
            }
            if (_rightRecognized && !_rightActive)
            {
                if (MLHands.Right.KeyPoseConfidence > _startConfidence)
                {
                    _rightActive = true;
                    if (OnGestureBegin != null)
                    {
                        OnGestureBegin(MLHands.Right);
                    }
                }
            }

            //OnGestureStay
            if (_leftActive && OnGestureStay != null)
            {
                OnGestureStay(MLHands.Left);
            }
            if (_rightActive && OnGestureStay != null)
            {
                OnGestureStay(MLHands.Right);
            }

            _lastLeftKeyPose = MLHands.Left.KeyPose;
            _lastRightKeyPose = MLHands.Right.KeyPose;
        }

        private void OnHandKeyPoseBegin(MLHandKeyPose gestureType, MLHandType handType)
        {
            if (gestureType == MLHandKeyPose.NoHand)
            {
                return;
            }

            if (handType == MLHandType.Left)
            {
                _leftRecognized = true;
            }
            else
            {
                _rightRecognized = true;
            }
        }

        private void OnHandKeyPoseEnd(MLHandKeyPose gestureType, MLHandType handType)
        {
            if (gestureType == MLHandKeyPose.NoHand)
            {
                return;
            }

            if (handType == MLHandType.Left)
            {
                _leftRecognized = false;
            }
            else
            {
                _rightRecognized = false;
            }
        }

        //----------- Public Methods -----------

        public static bool Matches(MLHand hand, MLHandKeyPose keyPose, MLHandType handType)
        {
            return hand.KeyPose == keyPose && hand.HandType == handType;
        }

        public static bool Matches(MLHand hand, KeyPoseTypes _leftTypes, KeyPoseTypes _rightTypes)
        {
            if(hand.HandType == MLHandType.Left)
            {
                return Matches(hand, _leftTypes);
            }
            else
            {
                return Matches(hand, _rightTypes);
            }
        }

        /// <summary>
        /// Compare MLHandKeyPose, to our GestureTypes bitmask.
        /// </summary>
        public static bool Matches(MLHand hand, KeyPoseTypes keyPose)
        {
            return Matches(hand.KeyPose, keyPose);
        }

        /// <summary>
        /// Compares MLHandKeyPose to KeyPoseTypes and returns true if they match.
        /// </summary>
        public static bool Matches(MLHandKeyPose handKeyPose, KeyPoseTypes keyPoseTypes)
        {
            int g = (int)handKeyPose;
            KeyPoseTypes convertedKeyPose = (KeyPoseTypes)(1 << g);
            return (keyPoseTypes & convertedKeyPose) == convertedKeyPose;
        }

        /// <summary>
        /// Convenience function, returns true of both hands are contained in the given gestureTypes bitmask.
        /// </summary>
        public static bool Matches(MLHandKeyPose leftPose, MLHandKeyPose rightPose, KeyPoseTypes keyPose)
        {
            return Matches(leftPose, keyPose) && Matches(rightPose, keyPose);
        }

    }
}

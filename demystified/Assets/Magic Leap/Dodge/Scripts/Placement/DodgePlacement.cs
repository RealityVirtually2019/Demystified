// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using MagicLeap;
using MagicLeap.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace MagicKit
{
    /// <summary>
    /// Application specific wrapper around the placement system.
    /// </summary>
    [RequireComponent(typeof(Placement))]
    public class DodgePlacement : MonoBehaviour
    {
        //----------- Public Events -----------

        public event Action OnPlacedObjectsProximityEnter;
        public event Action OnPlacedObjectsProximityExit;
        public event Action OnPlacementConfirmed;
        public event Action OnPlacementNotAllowed;

        //----------- Private Members -----------

        [SerializeField] private GameObject _placementPrefab;
        [SerializeField] private float _placementTiltOffset;
        [SerializeField] private Vector3 _placementExtents = new Vector3(1, 1, 1);
        [SerializeField] private float _placementClearance;
        [SerializeField, BitMask(typeof(KeyPoseTypes))] private KeyPoseTypes _placementRequestHandPoses;

        private Placement _placement;
        private List<GameObject> _placedObjects = new List<GameObject>();
        private List<GameObject> _intersectedObjects = new List<GameObject>();

        private const float MaxAngleOffsetFromVerticalDeg = 10;

        //----------- MonoBehaviour Methods -----------

        private void Awake()
        {
            _placement = GetComponent<Placement>();
        }

        private void OnEnable()
        {
            _placement.volume = _placementExtents;
            _placement.tilt = _placementTiltOffset;
            _placement.allowHorizontal = true;
            _placement.allowVertical = false;
            _placement.Place(Camera.main.transform, HandlePlacementConfirmed, IsPlacementValid);

            foreach (GameObject placedObject in _placedObjects)
            {
                ProximityIndicator proximitySphere = placedObject.GetComponent(typeof(ProximityIndicator)) as ProximityIndicator;
                if (proximitySphere != null)
                {
                    proximitySphere.enabled = true;
                    proximitySphere.SetRadius(_placementClearance);
                }
            }

            GestureManager.OnGestureBegin += HandleOnGestureBegin;
        }

        private void OnDisable()
        {
            foreach (GameObject placedObject in _placedObjects)
            {
                ProximityIndicator proximitySphere = placedObject.GetComponent(typeof(ProximityIndicator)) as ProximityIndicator;
                if (proximitySphere != null)
                {
                    proximitySphere.enabled = false;
                }
            }
            foreach (var sp in SpawnPointManager.Instance.spawnPoints)
            {
                sp.ShowPlacementIndicator(false);
            }

            GestureManager.OnGestureBegin -= HandleOnGestureBegin;
        }

        private void Update()
        {
            // Updates our list of placed objects intersected by the desired placement location, and notifies observers.
            Vector3 loc = _placement.Position;
            int prevCount = _intersectedObjects.Count;
            _intersectedObjects = GatherPlacedObjectsWithinRange(loc, _placementClearance);
            int currCount = _intersectedObjects.Count;
            if (prevCount == 0 && currCount > 0)
            {
                if (OnPlacedObjectsProximityEnter != null)
                {
                    OnPlacedObjectsProximityEnter();
                }
            }
            else if (currCount == 0 && prevCount > 0)
            {
                if (OnPlacedObjectsProximityExit != null)
                {
                    OnPlacedObjectsProximityExit();
                }
            }

            // Sets overlapped state to false on all objects.
            foreach (GameObject placedObject in _placedObjects)
            {
                ProximityIndicator proximitySphere = placedObject.GetComponent(typeof(ProximityIndicator)) as ProximityIndicator;
                if (proximitySphere != null && proximitySphere.enabled)
                {
                    proximitySphere.SetOverlapped(false);
                }
            }

            // Sets overlapped state to true for intersected objects.
            foreach (GameObject placedObject in _intersectedObjects)
            {
                ProximityIndicator proximitySphere = placedObject.GetComponent(typeof(ProximityIndicator)) as ProximityIndicator;
                if (proximitySphere != null && proximitySphere.enabled)
                {
                    proximitySphere.SetOverlapped(true);
                    proximitySphere.overlapperPosition = loc;
                }
            }
        }

        //----------- Private Methods -----------

        private void HandleOnGestureBegin(MLHand hand)
        {
            if (GestureManager.Matches(hand, _placementRequestHandPoses))
            {
                RequestPlacement();
            }
        }

        private List<GameObject> GatherPlacedObjectsWithinRange(Vector3 loc, float distance)
        {
            List<GameObject> objectsInRange = new List<GameObject>();
            foreach (GameObject placedObject in _placedObjects)
            {
                Vector3 placedLoc = placedObject.transform.position;

                if (Vector3.Distance(loc, placedLoc) < distance)
                {
                    objectsInRange.Add(placedObject);
                }
            }

            return objectsInRange;
        }

        private bool RequestPlacement()
        {
            if (CanPlace())
            {
                ConfirmPlacement();
                return true;
            }

            if (OnPlacementNotAllowed != null)
            {
                OnPlacementNotAllowed();
            }
            return false;
        }

        private void ConfirmPlacement()
        {
            _placement.Confirm();
            _placement.volume = _placementExtents;
            _placement.tilt = _placementTiltOffset;
            _placement.allowHorizontal = true;
            _placement.allowVertical = false;
            _placement.Place(Camera.main.transform, HandlePlacementConfirmed, IsPlacementValid);
        }

        private void HandlePlacementConfirmed(Vector3 position, Quaternion rotation)
        {
            GameObject go = GameObject.Instantiate(_placementPrefab);
            go.transform.position = _placement.Position;
            go.transform.rotation = _placement.Rotation;

            _placedObjects.Add(go);

            // Forwards the event to our listeners.
            if (OnPlacementConfirmed != null)
            {
                OnPlacementConfirmed();
            }
        }

        private bool CanPlace()
        {
            return (_placement.Running && _placement.Fit == FitType.Fits && IsPlacementValid(_placement.Position, _placement.Rotation));
        }

        private bool IsPlacementValid(Vector3 position, Quaternion rotation)
        {
            return (IsPlacementRotationValid(rotation) && IsClearOfPlacedObjects(position));
        }

        private bool IsPlacementRotationValid(Quaternion rotation)
        {
            Vector3 placementUp = rotation * Vector3.up;
            float dot = Vector3.Dot(placementUp, Vector3.up);
            if (dot > Mathf.Cos(MaxAngleOffsetFromVerticalDeg * Mathf.Deg2Rad))
            {
                return true;
            }

            return false;
        }

        private bool IsClearOfPlacedObjects(Vector3 location)
        {
            foreach (GameObject placedObject in _placedObjects)
            {
                Vector3 placedLoc = placedObject.transform.position;

                if (Vector3.Distance(location, placedLoc) < _placementClearance)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
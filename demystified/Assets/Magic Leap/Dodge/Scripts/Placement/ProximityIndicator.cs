// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using UnityEngine;

namespace MagicKit
{
    /// <summary>
    /// A visual feedback object driven by the Placement system. It's purpose is to provide feedback
    /// to the user when an object they are trying to place is too close to an object they previously placed.
    /// </summary>
    public class ProximityIndicator : MonoBehaviour
    {
        //----------- Public Members -----------

        [HideInInspector] public Vector3 overlapperPosition;

        //----------- Private Members -----------

        [SerializeField] private GameObject _visualPrefab;
        [SerializeField] private LineRenderer _linePrefab;
        private float _radius = 1;
        private GameObject _visual;
        private LineRenderer _lineToOverlapper;
        private bool _overlapped;

        //----------- MonoBehaviour Methods -----------

        private void Awake()
        {
            _visual = Instantiate(_visualPrefab, transform, false);

            _lineToOverlapper = Instantiate(_linePrefab, transform, false);
            _lineToOverlapper.positionCount = 2;

            SetRadius(_radius);
        }

        private void OnEnable()
        {
            SetOverlappedInternal(_overlapped);
        }

        private void OnDisable()
        {
            SetOverlappedInternal(false);
        }

        private void Update()
        {
            _lineToOverlapper.SetPosition(0, transform.position);
            _lineToOverlapper.SetPosition(1, overlapperPosition);
        }

        //----------- Public Methods -----------

        /// <summary>
        /// Set the sphere's radius.
        /// </summary>
        public void SetRadius(float radius)
        {
            _radius = radius;
            _visual.transform.localScale = new Vector3(radius, radius, radius);
        }

        /// <summary>
        /// Set the overlapped state for the proximity sphere.
        /// The visual is not shown unless it is being overlapped.
        /// </summary>
        public void SetOverlapped(bool overlapped)
        {
            _overlapped = overlapped;
            SetOverlappedInternal(overlapped);
        }

        //----------- Private Methods -----------

        private void SetOverlappedInternal(bool overlapped)
        {
            _visual.SetActive(overlapped);
            _lineToOverlapper.enabled = overlapped;
        }
    }
}

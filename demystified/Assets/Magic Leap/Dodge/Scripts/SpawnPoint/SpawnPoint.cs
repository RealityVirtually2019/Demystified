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
    /// An object that the golem can set its location to if it is not currently occupied.
    /// </summary>
    public class SpawnPoint : MonoBehaviour
    {
        //----------- Public Members -----------

        public bool Available
        {
            get
            {
                return _occupant == null;
            }
        }

        //----------- Private Members -----------

        [SerializeField] private GameObject _placementIndicatorPrefab;
        [SerializeField] private Animator _animatedVisualPrefab;

        private Transform _occupant;
        private Animator _animController;
        private GameObject _placementIndicator;

        private const string SpawnAvailableAnim = "Fill";

        //----------- MonoBehaviour Methods -----------

        private void Awake()
        {
            // Visual is disabled and placement indicator is enabled by default.
            _animController = Instantiate(_animatedVisualPrefab, transform, false);
            _animController.gameObject.SetActive(false);
            _placementIndicator = Instantiate(_placementIndicatorPrefab, transform, false);
            _occupant = null;
            SpawnPointManager.Instance.RegisterSpawnPoint(this);
        }

        //----------- Public Methods -----------

        /// <summary>
        /// Check if this spawn point is available and if so, set the occupant to the requester.
        /// </summary>
        public bool RequestSpawnPoint(Transform occupant)
        {
            if (_occupant != null)
            {
                return false;
            }
            _occupant = occupant;
            _occupant.position = transform.position;
            // Update the visual. 
            _animController.gameObject.SetActive(true);
            _animController.transform.position = _occupant.transform.position;
            _animController.SetBool(SpawnAvailableAnim, false);
            return true;
        }

        /// <summary>
        /// Empty the spawn point to make it available.
        /// </summary>
        public void ReleaseSpawnPoint()
        {
            if(_occupant == null)
            {
                return;
            }
            _occupant = null;
            _animController.SetBool(SpawnAvailableAnim, true);
        }

        /// <summary>
        /// Toggles whether the placement indicator is displayed for the spawn point.
        /// </summary>
        public void ShowPlacementIndicator(bool status)
        {
            if (_placementIndicator == null)
            {
                return;
            }
            _placementIndicator.SetActive(status);
        }
    }
}


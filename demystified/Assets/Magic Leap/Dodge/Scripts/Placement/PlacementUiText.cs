// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace MagicKit
{
    /// <summary>
    /// A visual feedback object driven by the Placement system. It's purpose is to provide
    /// information and context on the Placement system's current state.
    /// </summary>
    public class PlacementUiText : MonoBehaviour
    {
        //----------- Private Members -----------

        private const string InvalidStringNotFit = "Invalid placement";
        private const string InvalidStringProximity = "Too close";

        [SerializeField] private DodgePlacement _dodgePlacement;
        [SerializeField] private Text _instructionalTextValid;
        [SerializeField] private Text _instructionalTextInvalid;

        private int _numPlaced;

        //----------- MonoBehaviour Methods -----------

        private void OnEnable()
        {
            _dodgePlacement.OnPlacementConfirmed += HandlePlacementConfirmed;
            _dodgePlacement.OnPlacedObjectsProximityEnter += HandlePlacedObjectsProximityEnter;
            _dodgePlacement.OnPlacedObjectsProximityExit += HandlePlacedObjectsProximityExit;

            _instructionalTextValid.text = BuildPlacementCountDisplayString();
            _instructionalTextInvalid.text = InvalidStringNotFit;
        }

        private void OnDisable()
        {
            _dodgePlacement.OnPlacementConfirmed -= HandlePlacementConfirmed;
            _dodgePlacement.OnPlacedObjectsProximityEnter -= HandlePlacedObjectsProximityEnter;
            _dodgePlacement.OnPlacedObjectsProximityExit -= HandlePlacedObjectsProximityExit;
        }

        //----------- Event Handlers -----------

        private void HandlePlacementConfirmed()
        {
            _numPlaced++;
            _instructionalTextValid.text = BuildPlacementCountDisplayString();
        }

        private void HandlePlacedObjectsProximityEnter()
        {
            _instructionalTextInvalid.text = InvalidStringProximity;
        }

        private void HandlePlacedObjectsProximityExit()
        {
            _instructionalTextInvalid.text = InvalidStringNotFit;
        }

        //----------- Private Methods -----------

        private string BuildPlacementCountDisplayString()
        {
            int numObjectsToPlace = SpawnPointManager.Instance.maxSpawnPoints - _numPlaced;
            return string.Format("Pinch to place (x{0})", numObjectsToPlace);
        }
    }
}



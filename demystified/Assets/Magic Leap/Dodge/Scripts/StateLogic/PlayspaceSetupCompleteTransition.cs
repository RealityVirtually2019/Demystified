// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using MagicLeap.Utilities;
using UnityEngine;

namespace MagicKit
{
    /// <summary>
    /// State machine transition that waits until Placement is complete before going to the next state.
    /// </summary>
    [RequireComponent(typeof(State))]
    public class PlayspaceSetupCompleteTransition : MonoBehaviour
    {
        //----------- Private Members -----------

        [SerializeField] private State _nextState;
        [SerializeField] private DodgePlacement _dodgePlacement;

        private State _currentState;
        private int _numPlaced;

        //----------- MonoBehaviour Methods -----------

        private void Awake()
        {
            _currentState = GetComponent<State>();
        }

        private void OnEnable()
        {
            _numPlaced = 0;
            _dodgePlacement.OnPlacementConfirmed += HandlePlacementConfirmed;
        }

        private void OnDisable()
        {
            _dodgePlacement.OnPlacementConfirmed -= HandlePlacementConfirmed;
        }

        //----------- Event Handlers -----------

        private void HandlePlacementConfirmed()
        {
            _numPlaced++;

            if(_numPlaced >= SpawnPointManager.Instance.maxSpawnPoints)
            {
                _currentState.parentStateMachine.GoToState(_nextState.gameObject.name);
            }
        }
    }
}
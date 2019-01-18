// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using UnityEngine;
using MagicLeap.Utilities;

namespace MagicKit
{
    /// <summary>
    /// A state whose purpose is to wait a specified amount of time before transitioning to the next state.
    /// </summary>
    [RequireComponent(typeof(State))]
    public class TimerTransition : MonoBehaviour
    {
        //----------- Private Members -----------

        [SerializeField] private State _nextState;
        [SerializeField] private float _transitionDelay;

        private float _timer;
        private State _currentState;

        //----------- MonoBehaviour Methods -----------

        private void Awake()
        {
            _currentState = GetComponent<State>();
        }

        private void OnEnable()
        {
            _timer = Time.time + _transitionDelay;
        }

        private void Update()
        {
            if (Time.time > _timer)
            {
                _currentState.parentStateMachine.GoToState(_nextState.gameObject.name);
            }
        }
    }
}

// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace MagicKit
{
    /// <summary>
    /// Decision maker for golem behavior.
    /// </summary>
    public class GolemAi : MonoBehaviour
    {
        //----------- Private Members -----------

        [SerializeField] private Player _player;
        [SerializeField] private Golem _golem;
        // The maximum distance (m) a player can get before this entity enters the scared state.
        [SerializeField] private float _playerScaredRange = 1.25f;
        // The maximum distance (m) a player can get before this entity enters the hidden state.
        [SerializeField] private float _playerHideRange = 0.75f;
        // The delay (s) after the golem has thrown a projectile before it can throw another.
        [SerializeField] private float _throwCooldown = 1.5f;
        // The delay (s) after the hide animation completes before the golem chooses a new spawn point.
        [SerializeField] private float _findSpawnDelay = 2f;

        private float _throwAvailableTime;
        private State _state;
        private bool _hidden;
        private SpawnPoint _spawnPoint;
        private bool _foundNewSpawnPoint;

        private const float SpawnRangeBuffer = 0.2f;
        private const string HideCompleteCallback = "DelayHandleOnHideComplete";

        private enum State
        {
            Intro,
            Idle,
            Scared,
            Hidden,
            Throw,
        }

        //----------- MonoBehaviour Methods -----------

        private void OnEnable()
        {
            _golem.Initialize();
            GoToState(State.Intro);
        }

        private void Update()
        {
            UpdateStateMachine();
        }

        //-----------Event Handlers -----------

        private void HandleOnIntroComplete()
        {
            GoToState(State.Idle);
        }

        private void HandleOnHideComplete()
        {
            // Release the current spawn point
            if(_spawnPoint != null)
            {
                _spawnPoint.ReleaseSpawnPoint();
            }
            // Wait before finding a new spawn point.
            Invoke(HideCompleteCallback, _findSpawnDelay);
        }

        private void HandleOnThrowComplete()
        {
            GoToState(State.Hidden);
        }

        //----------- State Machine Methods -----------

        private void GoToState(State newState)
        {
            switch (_state)
            {
                case State.Intro:
                    ExitIntro();
                    break;
                case State.Idle:
                    ExitIdle();
                    break;
                case State.Scared:
                    ExitScared();
                    break;
                case State.Hidden:
                    ExitHidden();
                    break;
                case State.Throw:
                    ExitThrow();
                    break;
            }
            _state = newState;
            switch (_state)
            {
                case State.Intro:
                    EnterIntro();
                    break;
                case State.Idle:
                    EnterIdle();
                    break;
                case State.Scared:
                    EnterScared();
                    break;
                case State.Hidden:
                    EnterHidden();
                    break;
                case State.Throw:
                    EnterThrow();
                    break;
            }
        }

        private void UpdateStateMachine()
        {
            switch (_state)
            {
                case State.Intro:
                    UpdateIntro();
                    break;
                case State.Idle:
                    UpdateIdle();
                    break;
                case State.Scared:
                    UpdateScared();
                    break;
                case State.Hidden:
                    UpdateHidden();
                    break;
                case State.Throw:
                    UpdateThrow();
                    break;
            }
        }

        //----------- Intro State Methods -----------

        private void EnterIntro()
        {
            TryGetFurthestSpawnPoint(ref _spawnPoint, transform, 0f);
            _golem.OnIntroComplete += HandleOnIntroComplete;
            _golem.SetIntro(true);
        }

        private void UpdateIntro()
        {
            if (InsideHideRange())
            {
                GoToState(State.Hidden);
            }
        }

        private void ExitIntro()
        {
            _golem.OnIntroComplete -= HandleOnIntroComplete;
            _golem.SetIntro(false);
        }

        //----------- Idle State Methods -----------

        private void EnterIdle()
        {
            CalculateThrowAvailableTime();
        }

        private void UpdateIdle()
        {
            if (InsideHideRange())
            {
                GoToState(State.Hidden);
            }
            else if (InsideScaredRange())
            {
                GoToState(State.Scared);
            }
            else if (CanThrow())
            {
                GoToState(State.Throw);
            }
        }

        private void ExitIdle()
        {
        }

        //----------- Throw State Methods -----------

        private void EnterThrow()
        {
            _golem.OnThrowComplete += HandleOnThrowComplete;
            _golem.Throw(_player.transform);
            CalculateThrowAvailableTime();
        }

        private void UpdateThrow()
        {
            if (InsideHideRange())
            {
                GoToState(State.Hidden);
            }
            else if (InsideScaredRange())
            {
                GoToState(State.Scared);
            }
        }

        private void ExitThrow()
        {
            _golem.OnThrowComplete -= HandleOnThrowComplete;
        }

        //----------- Scared State Methods -----------

        private void EnterScared()
        {
            _golem.SetScared(true);
            CalculateThrowAvailableTime();
        }

        private void UpdateScared()
        {
            if (InsideHideRange())
            {
                GoToState(State.Hidden);
            }
            else if (!InsideScaredRange())
            {
                GoToState(State.Idle);
            }
        }

        private void ExitScared()
        {
            _golem.SetScared(false);
        }

        //----------- Hidden State Methods -----------

        private void EnterHidden()
        {
            _hidden = false;
            _foundNewSpawnPoint = false;
            _golem.OnHideComplete += HandleOnHideComplete;
            _golem.SetHidden(true);
        }

        private void UpdateHidden()
        {
            if (!_hidden)
            {
                return;
            }
            if (!InsideHideRange() && !InsideScaredRange() && _foundNewSpawnPoint)
            {
                GoToState(State.Idle);
            }
            else
            {
                _foundNewSpawnPoint = TryGetFurthestSpawnPoint(ref _spawnPoint, transform, _playerScaredRange + SpawnRangeBuffer);
            }
        }

        private void ExitHidden()
        {
            _golem.OnHideComplete -= HandleOnHideComplete;
            _golem.SetHidden(false);
        }

        //-----------Private Methods -----------

        private void CalculateThrowAvailableTime()
        {
            _throwAvailableTime = Time.time + _throwCooldown;
        }

        private bool InsideHideRange()
        {
            float playerDist = Vector3.Distance(_player.transform.position, transform.position);
            float playerLeftHandDist = Vector3.Distance(_player.leftHandPosition, transform.position);
            float playerRightHandDist = Vector3.Distance(_player.rightHandPosition, transform.position);

            float testDist = playerDist;
            testDist = Mathf.Min(testDist, playerLeftHandDist);
            testDist = Mathf.Min(testDist, playerRightHandDist);
            return testDist < _playerHideRange;
        }

        private bool InsideScaredRange()
        {
            float playerDist = Vector3.Distance(_player.transform.position, transform.position);
            float playerLeftHandDist = Vector3.Distance(_player.leftHandPosition, transform.position);
            float playerRightHandDist = Vector3.Distance(_player.rightHandPosition, transform.position);

            float testDist = playerDist;
            testDist = Mathf.Min(testDist, playerLeftHandDist);
            testDist = Mathf.Min(testDist, playerRightHandDist);
            return testDist < _playerScaredRange;
        }

        private bool CanThrow()
        {
            return _throwAvailableTime < Time.time;
        }

        private void DelayHandleOnHideComplete()
        {
            // Logic that runs after the golem has hidden: sets a flag that is checked in the hidden state
            // and attempts to find a new spawn point.
            _foundNewSpawnPoint = TryGetFurthestSpawnPoint(ref _spawnPoint, transform, _playerScaredRange + SpawnRangeBuffer);
            _hidden = true;
        }

        private bool TryGetFurthestSpawnPoint(ref SpawnPoint spawnPoint, Transform occupant, float minDist)
        {
            // Finds the spawn point that is the furthest from the player and currently unoccupied.
            List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>();
            foreach (var sp in SpawnPointManager.Instance.spawnPoints)
            {
                if (sp.Available && sp != spawnPoint)
                {
                    availableSpawnPoints.Add(sp);
                }
            }
            if (availableSpawnPoints.Count > 0)
            {
                float furthestDist = 0f;
                SpawnPoint furthestPoint = null;
                foreach (var sp in availableSpawnPoints)
                {
                    float dist = Vector3.Distance(_player.transform.position, sp.transform.position);
                    if (dist > furthestDist && dist > minDist)
                    {
                        furthestDist = dist;
                        furthestPoint = sp;
                    }
                }
                if (furthestPoint != null)
                {
                    if (furthestPoint.RequestSpawnPoint(occupant))
                    {
                        if (spawnPoint != null)
                        {
                            spawnPoint.ReleaseSpawnPoint();
                        }
                        spawnPoint = furthestPoint;
                        return true;
                    }
                }
                else if (spawnPoint != null)
                {
                    // If there are no other valid spawn points, re-select the current one.
                    spawnPoint.RequestSpawnPoint(occupant);
                    return true;
                }
            }
            else if (spawnPoint != null)
            {
                // If there are no other available spawn points, re-select the current one.
                spawnPoint.RequestSpawnPoint(occupant);
                return true;
            }
            return false;
        }
    }
}
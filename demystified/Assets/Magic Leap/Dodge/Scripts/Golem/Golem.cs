// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using UnityEngine;
using System;

namespace MagicKit
{
    /// <summary>
    /// Manages the visual components for the golem.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(GolemAudioBehavior))]
    public class Golem : MonoBehaviour
    {

        //----------- Public Events -----------

        public event Action OnIntroComplete;
        public event Action OnHideComplete;
        public event Action OnThrowComplete;

        //----------- Private Members -----------

        [SerializeField] private Player _player;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private GameObject _rockTransitionPrefab;
        [SerializeField] private GameObject _rockInterruptionPrefab;

        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private Transform _throwingHand;
        [SerializeField] private Transform _throwTransitionSpawnPos;

        private Animator _animator;
        private GolemAudioBehavior _audioBehavior;
        private bool _throwing;
        private bool _facingPlayer;
        private GameObject _rockTransition;
        private Projectile _projectile;
        private Transform _throwTarget;
        private bool _initialized;
        private const float DestroyGroundEffectDelay = 5f;
        private const string IntroAnim = "PlayIntro";
        private const string SpawnAnim = "Spawn";
        private const string ScaredreactAnim = "ReactOuter";
        private const string ScaredAnim = "PlayerInCircle";
        private const string HidereactAnim = "ReactInner";
        private const string ThrowAnim = "BoulderToss";
        private const string IntroCompleteKey = "IntroComplete";
        private const string HideCompleteKey = "HideComplete";
        private const string ThrowCompleteKey = "ThrowComplete";
        private const string PickupBoulderKey = "PickupBoulder";
        private const string ReleaseBoulderKey = "ReleaseBoulder";

        //----------- MonoBehaviour Methods -----------

        private void LateUpdate()
        {
            if (_facingPlayer)
            {
                FacePlayer(true);
            }
        }

        //----------- Event Handlers -----------

        public void HandleGolemAnimationEvent(string key)
        {
            switch (key)
            {
                case IntroCompleteKey:
                    var introHandler = OnIntroComplete;
                    if (introHandler != null)
                    {
                        introHandler();
                    }
                    _facingPlayer = true;
                    break;
                case HideCompleteKey:
                    var hideHandler = OnHideComplete;
                    if (hideHandler != null)
                    {
                        hideHandler();
                    }
                    _facingPlayer = true;
                    break;
                case ThrowCompleteKey:
                    var throwHandler = OnThrowComplete;
                    if (throwHandler != null)
                    {
                        throwHandler();
                    }
                    break;
                case PickupBoulderKey:
                    if (!_throwing)
                    {
                        return;
                    }
                    if (_rockTransition != null)
                    {
                        Destroy(_rockTransition);
                    }
                    _projectile = Instantiate(_projectilePrefab, _throwingHand, false);
                    _projectile.transform.localPosition = Vector3.zero;
                    _facingPlayer = true;
                    break;
                case ReleaseBoulderKey:
                    if (!_throwing)
                    {
                        return;
                    }
                    if (_projectile != null)
                    {
                        _projectile.transform.SetParent(null);
                        _projectile.Init(_throwTarget);
                        _projectile = null;
                    }
                    _throwing = false;
                    break;
            }
        }

        //----------- Public Methods -----------

        /// <summary>
        /// Spawn the prefabs needed for the avatar.
        /// </summary>
        public void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            _animator = GetComponent<Animator>();
            _audioBehavior = GetComponent<GolemAudioBehavior>();
            _initialized = true;
        }

        /// <summary>
        /// Start the animation event sequence to execute a throw.
        /// </summary>
        public void Throw(Transform target)
        {
            if (_throwing)
            {
                return;
            }
            _throwTarget = target;
            _facingPlayer = false;
            _throwing = true;
            if (_rockTransition != null)
            {
                Destroy(_rockTransition);
            }
            _rockTransition = Instantiate(_rockTransitionPrefab, _throwTransitionSpawnPos);
            SetThrowInternal(true);
        }

        /// <summary>
        /// Plays the intro animation.
        /// </summary>
        public void SetIntro(bool status)
        {
            if (status)
            {
                FacePlayer(false);
                _facingPlayer = false;
                SetIntroInternal(true);
            }
            else
            {
                SetIntroInternal(false);
            }
        }

        /// <summary>
        /// Set or unset the scared visual state.
        /// </summary>
        public void SetScared(bool status)
        {
            if (status)
            {
                SetInterruptThrowInternal();
                SetScaredInternal(true);
            }
            else
            {
                SetScaredInternal(false);
            }
        }

        /// <summary>
        /// Set or unset the hide visual state.
        /// </summary>
        public void SetHidden(bool status)
        {
            if (status)
            {
                SetInterruptThrowInternal();
                SetSpawnInternal(false);
                _facingPlayer = false;
                SetHideInternal(true);
            }
            else
            {
                SetSpawnInternal(true);
                SetHideInternal(false);
            }
        }

        //----------- Private Methods -----------

        private void SetInterruptThrowInternal()
        {
            if (!_throwing)
            {
                return;
            }
            _throwing = false;
            _facingPlayer = true;
            SetThrowInternal(false);
            if(_rockTransition != null)
            {
                Destroy(_rockTransition);

                // Rock interruption object manages its own destruction.
                Instantiate(_rockInterruptionPrefab, _rockTransition.transform.position, _rockTransition.transform.rotation);
            }
            if (_projectile != null)
            {
                Destroy(_projectile.gameObject);

                // Rock interruption object manages its own destruction.
                Instantiate(_rockInterruptionPrefab, _projectile.transform.position, _projectile.transform.rotation);
            }
            if (_audioBehavior != null)
            {
                _audioBehavior.InterruptThrow();
            }
        }

        private void SetIntroInternal(bool status)
        {
            if (status)
            {
                _animator.SetTrigger(IntroAnim);
            }
            else
            {
                _animator.ResetTrigger(IntroAnim);
            }
        }

        private void SetThrowInternal(bool status)
        {
            if (status)
            {
                _animator.SetTrigger(ThrowAnim);
            }
            else
            {
                _animator.ResetTrigger(ThrowAnim);
            }
        }

        private void SetScaredInternal(bool status)
        {
            if (status)
            {
                _animator.SetTrigger(ScaredreactAnim);
                _animator.SetBool(ScaredAnim, true);
            }
            else
            {
                _animator.ResetTrigger(ScaredreactAnim);
                _animator.SetBool(ScaredAnim, false);
            }
        }

        private void SetHideInternal(bool status)
        {
            if (status)
            {
                _animator.SetTrigger(HidereactAnim);
            }
            else
            {
                _animator.ResetTrigger(HidereactAnim);
            }
        }

        private void SetSpawnInternal(bool status)
        {
            if (status)
            {
                _animator.SetTrigger(SpawnAnim);
            }
            else
            {
                _animator.ResetTrigger(SpawnAnim);
            }
        }

        private void FacePlayer(bool lerp)
        {
            Vector3 lookDir = Vector3.ProjectOnPlane((_player.transform.position - transform.position).normalized, Vector3.up);
            Quaternion lookRot = Quaternion.LookRotation(lookDir);
            if (lerp)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * _rotationSpeed);
            }
            else
            {
                transform.rotation = lookRot;
            }
        }
    }
}

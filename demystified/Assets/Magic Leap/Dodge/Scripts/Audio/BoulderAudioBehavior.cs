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
    /// Boulder audio behavior.
    /// </summary>
    public class BoulderAudioBehavior : AudioBehavior
    {
        //----------- Private Members -----------

        // Time in seconds from the start of the audio clip to the peak whoosh sound.
        [SerializeField] private float _passbyTimeToPeak = 0.562f;
        // How long should we wait before triggering the next passby on this object.
        [SerializeField] private float _passbyCooldown = 5f;
        [SerializeField] private AudioSource _collisionAudioSource;

        private Vector3 _lastPosition;
        private float _lastDistanceToListener;
        private float _passbyTimer;
        private AudioListener _audioListener;
        private static AudioSource _collisionAudioSourceRef;
        private const string PassBySound = "passby";
        private const string SpinSound = "spin";
        private const string CollideSound = "boom";

        //----------- MonoBehaviour Methods -----------

        private void Start()
        {
            _lastPosition = transform.position;
            _audioListener = (AudioListener)FindObjectOfType(typeof(AudioListener));

            // If we play sound on collision, the Audio Source will get destroyed along with this object.
            // We'll move the collision audio source into the main hierarchy, but preserve the reference to it.
            if (_collisionAudioSourceRef == null)
            {
                _collisionAudioSource.transform.parent = null;
                _collisionAudioSourceRef = _collisionAudioSource;
            }
        }

        private void Update()
        {
            // Decrement cooldown timer as needed.
            if (_passbyTimer > 0f)
            {
                _passbyTimer -= Time.deltaTime;
            }

            // Am I moving towards the listener?
            float distanceToListener = Vector3.Distance(transform.position, _audioListener.transform.position);
            bool movingTowardsListener = (distanceToListener < _lastDistanceToListener);
            _lastDistanceToListener = distanceToListener;

            // What is my velocity?
            float velocity = (transform.position - _lastPosition).magnitude / Time.deltaTime;
            _lastPosition = transform.position;

            // Find the distance at which we need to trigger the passby sound.
            float passbyDistance = _passbyTimeToPeak * velocity;

            // Trigger passby if:
            //  - moving towards listener
            //  - at the right distance to "whoosh" at the right time
            //  - cooldown is not finished yet
            if (movingTowardsListener && _lastDistanceToListener < passbyDistance && _passbyTimer <= 0f)
            {
                _passbyTimer = _passbyCooldown;
                PlaySound(PassBySound);
            }
        }

        //----------- Public Methods -----------

        public void Spin()
        {
            PlaySound(SpinSound);
        }

        public void Collide(Vector3 collisionPosition)
        {
            _collisionAudioSourceRef.transform.position = collisionPosition;
            PlaySound(CollideSound, _collisionAudioSourceRef);
        }
    }
}

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
    /// Moves in the direction of its given target. It destroys itself when it collides with headpose
    /// or when it moves a set distance past headpose.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        //----------- Private Members -----------

        [SerializeField] private GameObject _visualPrefab;
        [SerializeField] private GameObject _dodgeEffect;
        [SerializeField] private Transform _visualRoot;
        [SerializeField] private BoulderAudioBehavior _boulderAudioBehavior;
        [SerializeField] private float _moveSpeed = 0.75f;
        [SerializeField] private float _rotationSpeed = 200f;
        //The distance (m) the projectile must travel past headpose before it destroys itself.
        [SerializeField] private float _distThreshold = 2f;

        // Play the Whoosh sound every X degrees of rotation.
        private const float WhooshDegrees = 180f;
        private Transform _target;
        private Vector3 _rotateDir;
        private float _totalRotation;
        private Vector3 _moveDir;
        private bool _initialized;

        //----------- MonoBehaviour Methods -----------

        private void Awake()
        {
            Instantiate(_visualPrefab, _visualRoot, false);
        }

        private void Update()
        {
            if (!_initialized)
            {
                return;
            }
            // Update the projectile position.
            transform.position += _moveDir * _moveSpeed * Time.deltaTime;
            _visualRoot.RotateAround(_visualRoot.position, _rotateDir, _rotationSpeed * Time.deltaTime);

            // Play sound for each rotation.
            _totalRotation += _rotationSpeed * Time.deltaTime;
            if (_totalRotation > WhooshDegrees)
            {
                _totalRotation = _totalRotation % WhooshDegrees;
                _boulderAudioBehavior.Spin();
            }

            // Check if the projectile has been dodged.
            float dist = Vector3.Distance(_target.position, transform.position);
            Vector3 dirToTarget = (_target.position - transform.position).normalized;
            float dot = Vector3.Dot(dirToTarget, _moveDir);
            if (dot < 0 && dist > _distThreshold)
            {
                DodgedProjectile();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // This will get called when the projectile collides with headpose.
            if (!_initialized)
            {
                return;
            }
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            // This will get called when the projectile collides with the mesh.
            if (!_initialized)
            {
                return;
            }
            DodgedProjectile();
        }

        //----------- Public Methods -----------

        /// <summary>
        /// Calculate the move and rotation direction towards the target.
        /// </summary>
        public void Init(Transform target)
        {
            _target = target;
            _moveDir = (_target.position - transform.position).normalized;
            _rotateDir = Vector3.Cross(Vector3.up, _moveDir);
            _initialized = true;
            _boulderAudioBehavior.Spin();
        }

        //----------- Private Methods -----------

        private void DodgedProjectile()
        {
            Instantiate(_dodgeEffect, transform.position, transform.rotation);
            _boulderAudioBehavior.Collide(transform.position);
            Destroy(gameObject);
        }
    }
}

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
    /// Models the in-game physical representation of the player.
    /// </summary>
    public class Player : MonoBehaviour
    {
        //----------- Public Members -----------

        [HideInInspector] public Vector3 rightHandPosition;
        [HideInInspector] public Vector3 leftHandPosition;

        //----------- Private Members -----------

        [SerializeField] private Collider _headCollider;
        [SerializeField] private Collider _leftHandCollider;
        [SerializeField] private Collider _rightHandCollider;
        [SerializeField] private GameObject _hitEffectPrefab;
        [SerializeField] private PlayerAudioBehavior _playerAudioBehavior;

        private const string CollideSound = "boom";

        //----------- MonoBehaviour Methods -----------

        private void OnTriggerEnter(Collider other)
        {
            Vector3 spawnPos = _headCollider.ClosestPoint(other.transform.position);
            Quaternion rot = Quaternion.LookRotation(transform.forward);
            Instantiate(_hitEffectPrefab, spawnPos, rot);
            _playerAudioBehavior.transform.position = spawnPos;
            _playerAudioBehavior.PlaySound(CollideSound);
        }

        private void Update()
        {
            leftHandPosition = GestureManager.LeftHand.Center;
            rightHandPosition = GestureManager.RightHand.Center;

            _leftHandCollider.transform.position = leftHandPosition;
            _rightHandCollider.transform.position = rightHandPosition;
        }
    }
}
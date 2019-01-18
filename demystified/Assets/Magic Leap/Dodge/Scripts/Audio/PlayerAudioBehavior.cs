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
    /// Player audio behavior.
    /// </summary>
    public class PlayerAudioBehavior : AudioBehavior
    {
        //----------- Private Members -----------

        [SerializeField] private DodgePlacement _dodgePlacement;

        private float _placementConfirmCooldownTimer;
        private const float PlacementConfirmCooldown = 0.5f;
        private const string GoodPlacementSound = "good_placement";
        private const string BadPlacementSound = "bad_placement";

        //----------- MonoBehaviour Methods -----------

        private void OnEnable()
        {
            _dodgePlacement.OnPlacementConfirmed += HandleOnPlacementConfirmed;
            _dodgePlacement.OnPlacementNotAllowed += HandleOnPlacementDenied;
        }

        private void OnDisable()
        {
            _dodgePlacement.OnPlacementConfirmed -= HandleOnPlacementConfirmed;
            _dodgePlacement.OnPlacementNotAllowed -= HandleOnPlacementDenied;
        }

        private void Update()
        {
            if (_placementConfirmCooldownTimer > 0f)
            {
                _placementConfirmCooldownTimer -= Time.deltaTime;
            }
        }

        //----------- Event Handlers -----------

        private void HandleOnPlacementConfirmed()
        {
            if (_placementConfirmCooldownTimer <= 0f)
            {
                PlaySound(GoodPlacementSound);
                _placementConfirmCooldownTimer = PlacementConfirmCooldown;
            }
        }

        private void HandleOnPlacementDenied()
        {
            if (_placementConfirmCooldownTimer <= 0f)
            {
                PlaySound(BadPlacementSound);
                _placementConfirmCooldownTimer = PlacementConfirmCooldown;
            }
        }
    }
}

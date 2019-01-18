// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using MagicLeap.Utilities;

namespace MagicKit
{
    /// <summary>
    /// Golem audio behavior.
    /// </summary>
    public class GolemAudioBehavior : AudioBehavior
    {
        //----------- Private Members -----------

        [SerializeField] private float _fadeOutDuration = 1f;

        private Dictionary<string, AudioSource> _soundsToSources;
        private const string BoulderThrowSound = "boulder_toss_start";

        //----------- MonoBehavior Methods -----------

        private void Awake()
        {
            _soundsToSources = new Dictionary<string, AudioSource>();
        }

        //----------- Event Handlers -----------

        public void HandleAudioAnimationEvent(string eventName)
        {
            if (_audioEventsInfo != null && _audioEventsInfo.ContainsKey(eventName))
            {
				AudioSource audioSource = PlaySound(eventName).audioSource;

                // Keep track of which source is playing which sound, so that we can fade it out if needed.
                if (_soundsToSources.ContainsKey(eventName))
                {
                    _soundsToSources[eventName] = audioSource;
                }
                else
                {
                    _soundsToSources.Add(eventName, audioSource);
                }
            }
        }

        //----------- Public Methods -----------

        public void InterruptThrow()
        {
            if (_soundsToSources.ContainsKey(BoulderThrowSound))
            {
                FadeOutAudio(_soundsToSources[BoulderThrowSound], _fadeOutDuration);
            }
        }
    }
}

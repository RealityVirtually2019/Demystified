// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicLeap.Utilities
{
	///<summary>
	/// Information about an event that's been prepared and assigned to an audio source.
	///</summary>
	public class AudioEvent
	{
		//----------- Public Members -----------

		public string eventName;
		public AudioSource audioSource;
		public AudioClip clip;
		public AudioBehavior audioBehavior;

		// When the event is prepared, initial pitch and volume are calculated based on combination of AudioSource settings
		// and user entered data for this event.
		public float volume;
		public float pitch;

		// Reference to fadeout coroutine, if currently fading out. Otherwise, null.
		public Coroutine fadeout;

		// Reference to fadein coroutine, if currently fading in. Otherwise, null.
		public Coroutine fadein;

        // Audio properties and current values.
        public Dictionary<AudioProperty, float> audioProperties = new Dictionary<AudioProperty, float>();

        /// <summary>
        /// Set audio property info (for debug purposes).
        /// </summary>
        /// <param name="audioProperty"></param>
        /// <param name="value"></param>
        public void StoreAudioPropertyValue(AudioProperty audioProperty, float value)
        {
            if (audioProperties.ContainsKey(audioProperty))
            {
                audioProperties[audioProperty] = value;
            }
            else
            {
                audioProperties.Add(audioProperty, value);
            }
        }
	}
}

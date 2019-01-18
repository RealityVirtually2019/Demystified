// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using UnityEngine;
#if MSA
using MSA;
#endif

namespace MagicLeap.Utilities
{
    //----------- Data Definitions -----------

    public enum AudioProperty
    {
        VOLUME,
        PITCH
    }

    ///<summary>
    /// Audio parameter.
    ///</summary>
    public class AudioParameter : MonoBehaviour
    {
        //----------- Public Events -----------

        // TODO: Figure out how will this work if there are multiple paremeters affecting the same audio property.
        public static void SetParameter(AudioEvent audioEvent, AudioParameterData audioParameterData, AudioSourceData audioSourceData, float parameterValue)
        {
            for (int i = 0; i < audioParameterData.audioPropertyCurves.Count; ++i)
            {
                AudioParameterData.AudioPropertyCurve audioPropertyCurve = audioParameterData.audioPropertyCurves[i];
                bool validAudioProperty = true;
                float value = audioPropertyCurve.curve.Evaluate(parameterValue);

                // TODO: Clamp values;
                switch (audioPropertyCurve.audioProperty)
                {
                    case AudioProperty.VOLUME:
                        audioEvent.audioSource.volume = audioEvent.volume * value;
                        break;

                    case AudioProperty.PITCH:
                        audioEvent.audioSource.pitch = audioEvent.pitch * value;
                        break;

                    default:
                        validAudioProperty = false;
                        break;
                }

                if (validAudioProperty)
                {
                    audioEvent.StoreAudioPropertyValue(audioPropertyCurve.audioProperty, value);
                }
            }
        }
    }
}


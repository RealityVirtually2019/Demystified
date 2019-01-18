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
    /// Enables GameObjects upon OnEnable and disables them upon OnDisable.
    /// </summary>
    public class GameObjectEnabler : MonoBehaviour
    {
        //----------- Private Members -----------

        // GameObjects that get turned on when the state is active and off when it is inactive. 
        [SerializeField] private GameObject[] _stateGameObjects;

        //----------- MonoBehaviour Methods -----------

        private void OnEnable()
        {
            SetStateObjectActiveStatus(true);
        }

        private void OnDisable()
        {
            SetStateObjectActiveStatus(false);
        }

        //----------- Private Methods -----------

        private void SetStateObjectActiveStatus(bool status)
        {
            foreach (GameObject go in _stateGameObjects)
            {
                if(go == null)
                {
                    continue;
                }
                go.SetActive(status);
            }
        }
    }
}
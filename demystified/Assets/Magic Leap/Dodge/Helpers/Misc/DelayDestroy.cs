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
    /// Destroys this GameObject at a delay after it has been instantiated.
    /// </summary>
    public class DelayDestroy : MonoBehaviour
    {
        //----------- Private Members -----------

        [SerializeField] private float _delay = 3f;

        //----------- MonoBehaviour Methods -----------

        private void Start()
        {
            Destroy(gameObject, _delay);
        }
    }
}
// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using MagicLeap.Utilities;

namespace MagicKit
{
    /// <summary>
    /// Entry point for access to golem spawn points.
    /// </summary>
    public class SpawnPointManager : Singleton<SpawnPointManager>
    {
        //----------- Public Members -----------

        public int maxSpawnPoints = 3;

        [HideInInspector] public List<SpawnPoint> spawnPoints;

        //----------- MonoBehaviour Methods -----------

        private void Awake()
        {
            spawnPoints = new List<SpawnPoint>();
        }

        //----------- Public Methods -----------

        /// <summary>
        /// Registers the spawn point to the app so operations can be performed on it later.
        /// </summary>
        public void RegisterSpawnPoint(SpawnPoint spawnPoint)
        {
            spawnPoints.Add(spawnPoint);
        }
    }
}

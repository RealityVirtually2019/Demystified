// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------
﻿using System.Collections.Generic;
using UnityEngine;
using System;

namespace MagicKit
{
    /// <summary>
    /// Listens for SpawnProp and DestroyProp animation events and inernally manages instantiation/attachment/destruction, etc.
    /// </summary>
    [RequireComponent (typeof (Animator))]
    public class AnimationPropManager : MonoBehaviour
    {
        //----------- Public Events -----------

        public event Action<GameObject> OnPropSpawned;

        //----------- Private Members -----------

        [System.Serializable]
        private class PropEventInfo
        {
            public string name = "";
            public GameObject prefab = null;
            public Transform parent = null;
            public bool attachToParent = false;
        }
        [SerializeField] private List<PropEventInfo> _propEventInfos = new List<PropEventInfo>();
        private Dictionary<string, PropEventInfo> _propEventInfoDict = new Dictionary<string, PropEventInfo>();
        private static readonly Dictionary<string, GameObject> _props =
            new Dictionary<string, GameObject>();

        //----------- MonoBehavior Methods -----------

        private void Awake()
        {
            // Populates prop event info dictionary.
            foreach (PropEventInfo info in _propEventInfos)
            {
                // Enforces uniqueness of keys.
                if (_propEventInfoDict.ContainsKey(info.name))
                {
                    Debug.LogWarning("Animation prop manager attempting to register duplicately named prop. Ignoring. " + info.name);
                }

                _propEventInfoDict.Add(info.name, info);
            }
        }

        //----------- Public Methods -----------

        public void SpawnProp(string key)
        {
            if (!_propEventInfoDict.ContainsKey(key))
            {
                Debug.LogWarning("SpawnProp animation event attempting to spawn unknown prop. Ignoring. " + key);
                return;
            }

            // If there is already an actively spawned prop by that name, we replace it.
            if (_props.ContainsKey(key))
            {
                GameObject.Destroy(_props[key]);
                _props.Remove(key);
            }

            PropEventInfo info = _propEventInfoDict[key];
            GameObject prop = Instantiate(info.prefab, info.parent.position, info.parent.rotation);
            if (info.attachToParent)
            {
                prop.transform.SetParent(info.parent);
            }
            prop.name = info.name;
            _props.Add(key, prop);

            var spawnHandler = OnPropSpawned;
            if (spawnHandler != null)
            {
                spawnHandler(prop);
            }
        }

        public void DestroyProp(string key)
        {
            if (!_props.ContainsKey(key))
            {
                return;
            }

            GameObject.Destroy(_props[key]);
            _props.Remove(key);
        }
    }
}

// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace MagicKit
{
    /// <summary>
    /// Swaps the mesh's material when the gameobject this script is attached to becomes enabled.
    /// Resets the mesh's material back to what it was previously when the gameobject this script
    /// is attached to becomes disabled.
    /// </summary>
    public class MeshMaterialSwapper : MonoBehaviour
    {
        //----------- Private Members -----------

        [SerializeField] private Material _swapMaterial;
        [SerializeField] private MLSpatialMapper _spatialMapper;
        [SerializeField] private Transform _meshParent;

        private Material _originalMaterial;
        private bool _initialized;

        //----------- MonoBehaviour Methods -----------

        private void OnEnable()
        {
            CheckInit();
            SwapMaterial(_swapMaterial);
        }

        private void OnDisable()
        {
            CheckInit();
            SwapMaterial(_originalMaterial);
        }

        //----------- Private Methods -----------

        private void CheckInit()
        {
            if (!_initialized)
            {
                Init();
            }
        }

        private void Init()
        {
            _originalMaterial = _spatialMapper.meshPrefab.GetComponent<MeshRenderer>().material;
            _initialized = true;
        }

        private void SwapMaterial(Material mat)
        {
            if(_meshParent == null)
            {
                return;
            }

            MeshRenderer[] meshRenderers = _meshParent.GetComponentsInChildren<MeshRenderer>(true);
            foreach(MeshRenderer rend in meshRenderers)
            {
                rend.material = mat;
            }
        }
    }
}

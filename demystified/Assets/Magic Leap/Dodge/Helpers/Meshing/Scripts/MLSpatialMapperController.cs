// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

using UnityEngine;
using UnityEngine.XR.MagicLeap;
using System.Collections.Generic;

namespace MagicKit
{
    ///<summary>
    /// This class organically grows the spatial mapper bounds (localScale) depending on the states of the spatialObjects.
    ///</summary>
    [RequireComponent(typeof(MLSpatialMapper))]
    public class MLSpatialMapperController : MonoBehaviour
    {

        //----------- Public Members -----------

        public List<SpatialObject> spatialObjects; 
        public bool removeOutsideExtents;

        //----------- Private Members -----------

        private Bounds _bounds;
        private Vector3 _initialSize;

        //----------- MonoBehaviour Methods -----------

        private void Start()
        {
            _initialSize = transform.localScale;
            ResetBounds();
        }

        private void Update()
        {
            if (removeOutsideExtents)
            {
                ResetBounds();
            }

            // Encapsulate the volumes of all objects
            foreach(SpatialObject sp in spatialObjects)
            {
                _bounds.Encapsulate(sp.bounds);
            }

            // Adjust the values
            transform.localScale = _bounds.size;
            transform.position = _bounds.center;
        }

        //----------- Public Methods -----------

        /// <summary>
        /// Add a spatial Object
        /// </summary>
        public void AddSpatialObject(Transform obj, Vector3 extents)
        {

            foreach (SpatialObject sp in spatialObjects)
            {
                if(obj == sp.objectTransform)
                {
                    Debug.LogError("Object already in list");
                    return;
                }
            }
            spatialObjects.Add(new SpatialObject(obj, extents));
        }

        /// <summary>
        /// Remove a spatial Object
        /// </summary>
        public void RemoveSpatialObject(Transform obj)
        {
            foreach(SpatialObject sp in spatialObjects)
            {
                if (obj == sp.objectTransform)
                {
                    spatialObjects.Remove(sp);
                    return;
                }
            }
        }

        //----------- Private Methods -----------

        private void ResetBounds()
        {
            _bounds = new Bounds(transform.position, _initialSize);
        }
    }
}
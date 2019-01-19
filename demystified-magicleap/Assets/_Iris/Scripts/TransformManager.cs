using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformManager : MonoBehaviour {

    public void DeChildMe(){

        this.transform.parent = null;
    }

    public void AssignNewParent(Transform newParent){

        this.transform.parent = newParent;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
    }
}

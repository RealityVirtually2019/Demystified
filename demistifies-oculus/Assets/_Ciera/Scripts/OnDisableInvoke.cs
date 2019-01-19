using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class OnDisableInvoke : MonoBehaviour {

    public UnityEvent onDisable;


    public void OnDisable(){

        onDisable.Invoke();
    }
}

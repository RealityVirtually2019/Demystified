using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.XR.MagicLeap;
[RequireComponent(typeof(ControllerConnectionHandler))]
public class OnTriggerPlayUnityEvent : MonoBehaviour
{

    public UnityEvent OnTrigger;
    private ControllerConnectionHandler _controllerConnectionHandler;


    private void Start()
    {
        _controllerConnectionHandler = GetComponent<ControllerConnectionHandler>();
        MLInput.OnTriggerDown += HandleOnTriggerDown;
    }

    private void HandleOnTriggerDown(byte controllerId, float value)
    {
        OnTrigger.Invoke();
    }


}

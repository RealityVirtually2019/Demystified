using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableInvoke : MonoBehaviour {

    public UnityEvent onEnable;

	public void OnEnable()
	{
        onEnable.Invoke(); 
	}
}

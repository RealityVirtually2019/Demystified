using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableInvoke : MonoBehaviour {

    public bool addDelay = false;
    public float delay = 3; 
    public UnityEvent onEnable;

	public void OnEnable()
	{
        if (!addDelay)
        {
            onEnable.Invoke();
        }
        else{
            StartCoroutine(WaitThenInvoke());
        }
	}

    IEnumerator WaitThenInvoke()
    {
        yield return new WaitForSeconds(delay);
        onEnable.Invoke(); 
    }
}

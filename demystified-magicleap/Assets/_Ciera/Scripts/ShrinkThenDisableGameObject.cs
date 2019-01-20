using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkThenDisableGameObject : MonoBehaviour {

	Vector3 StartSizeOfObjectToGrow;
	public float TimeOfChange = 1; 
	// Use this for initialization
	void OnEnable () {
	
		StartCoroutine (ShrinkingDown ());
	}

	IEnumerator ShrinkingDown ()
	{

		StartSizeOfObjectToGrow = transform.localScale;

		float progress = 0;
		while (progress < 1)
		{
			transform.localScale = Vector3.Slerp (transform.localScale, Vector3.zero, progress);

			progress += Time.deltaTime / TimeOfChange;
			yield return null;
		}

		gameObject.SetActive (false);
		transform.localScale = StartSizeOfObjectToGrow;

		yield return null;

	}
}

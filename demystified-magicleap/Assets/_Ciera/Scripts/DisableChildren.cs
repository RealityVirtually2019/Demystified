using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 
using UnityEditor;
[CustomEditor(typeof(DisableChildren))]
public class DisableChildrenEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		DisableChildren myScript = (DisableChildren)target;

		if (GUILayout.Button("Disable Children"))
		{
			myScript.DisableAllChildren ();
		}

	}
}
#endif 
public class DisableChildren : MonoBehaviour
{
	public void DisableChildrenExceptFor (Transform EnableThisChild)
	{
		if (EnableThisChild != null) { 
			for (int i = 0; i < transform.childCount; i++) {

				var currentChild = transform.GetChild (i);

				if (currentChild == EnableThisChild) {
					transform.GetChild (i).gameObject.SetActive (true);
				} else {
					transform.GetChild (i).gameObject.SetActive (false);
				}
			}
		} 
	}

	public void DisableAllChildren ()
	{
		for (int i = 0; i < transform.childCount; i++) {

			transform.GetChild (i).gameObject.SetActive (false);
		}
	}
    public void EnableAllChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {

            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}

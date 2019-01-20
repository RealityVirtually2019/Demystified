using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(OnCollision))]
public class OnCollisionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        OnCollision myScript = (OnCollision)target;

        if (GUILayout.Button("COllisionxs"))
        {
            myScript.onCollisionInvoke.Invoke();

        }



    }
}
#endif

public class OnCollision : MonoBehaviour {


    public enum CollisionType {onEnter, onExit}
    public CollisionType collisionType;

    public UnityEvent onCollisionInvoke;

	public void OnCollisionEnter(Collision collision)
	{
        if(collisionType == CollisionType.onEnter){
            onCollisionInvoke.Invoke(); 
        }
	}

    public void OnCollisionExit(Collision collision)
    {
        if (collisionType == CollisionType.onExit)
        {
            onCollisionInvoke.Invoke();
        }
    }
}

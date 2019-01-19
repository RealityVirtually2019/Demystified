using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TransformToNewTransform))]
public class TransformToNewTransformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TransformToNewTransform myScript = (TransformToNewTransform)target;

        if (GUILayout.Button("Go To Transform"))
        {
            myScript.GoToNewTransform();
        }
        if (GUILayout.Button("Revert Transform"))
        {
            myScript.GoToInitialTransform();
        }

    }
}
#endif
public class TransformToNewTransform : MonoBehaviour
{
    public Transform MoveMe, MoveThemToMe;
    Vector3 StartScale, StartPosition;
    Quaternion StartRotation;
    public float timeOfTransition = 1;
    public bool matchPosition = true, matchRotation = false, matchScale = false;

    void Start()
    {
        StartScale = MoveMe.localScale;
        StartPosition = MoveMe.position;
        StartRotation = MoveMe.rotation;
    }

    public void GoToThisNewTranform(Transform newTransform)
    {
        MoveThemToMe = newTransform;
        GoToNewTransform();
    }

    void OnDisable()
    {

        StopAllCoroutines();
    }
    public void GoToThisNewTranformInstantly(Transform newTransform)
    {
        StopAllCoroutines();
        MoveThemToMe = newTransform;
        if (matchPosition)
        {
            MoveMe.position = MoveThemToMe.position;
        }
        if (matchRotation)
        {
            MoveMe.rotation = MoveThemToMe.rotation;
        }
        if (matchScale)
        {
            MoveMe.localScale = MoveThemToMe.localScale;
        }
    }

    public void GoInitialTranformInstantly()
    {
        StopAllCoroutines();
        if (matchPosition)
        {
            MoveMe.position = StartPosition;
        }
        if (matchRotation)
        {
            MoveMe.rotation = StartRotation;
        }
        if (matchScale)
        {
            MoveMe.localScale = StartScale;
        }
    }

    public void GoToNewTransform()
    {
        StopAllCoroutines();
        StartCoroutine(GoingToNewTransform(MoveThemToMe.position, MoveThemToMe.rotation, MoveThemToMe.localScale));
    }
    public void GoToInitialTransform()
    {
        StopAllCoroutines();
        StartCoroutine(GoingToNewTransform(StartPosition, StartRotation, StartScale));
    }
    public void GoToInitialTransformInstantly()
    {
        StopAllCoroutines();
        if (matchPosition)
        {
            MoveMe.position = StartPosition;
        }
        if (matchRotation)
        {
            MoveMe.rotation = StartRotation;
        }
        if (matchScale)
        {
            MoveMe.localScale = StartScale;
        }
    }
    IEnumerator GoingToNewTransform(Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
    {
        float elapsedTime = 0;
        while (elapsedTime < timeOfTransition)
        {
            elapsedTime += Time.deltaTime;
            if (matchPosition)
            {
                MoveMe.position = Vector3.Lerp(MoveMe.position, newPosition, Mathf.SmoothStep(0, 1, elapsedTime / timeOfTransition));
            }
            if (matchRotation)
            {
                MoveMe.rotation = Quaternion.Lerp(MoveMe.rotation, newRotation, Mathf.SmoothStep(0, 1, elapsedTime / timeOfTransition));
            }
            if (matchScale)
            {
                MoveMe.localScale = Vector3.Lerp(MoveMe.localScale, newScale, Mathf.SmoothStep(0, 1, elapsedTime / timeOfTransition));
            }
            yield return null;

        }
        yield return null;
    }
}
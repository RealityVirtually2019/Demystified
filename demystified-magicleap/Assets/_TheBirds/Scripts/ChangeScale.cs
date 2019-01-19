using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ChangeScale))]
public class ChangeScaleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChangeScale myScript = (ChangeScale)target;

        if (GUILayout.Button("Grow"))
        {
            myScript.Grow();
        }

        if (GUILayout.Button("Shrink"))
        {
            myScript.Shrink();
        }

        if (GUILayout.Button("Assign local scale to \"GrowToScale\""))
        {
            myScript.MakeCurrentScaleTheGrowToScale();
        }

        if (GUILayout.Button("Assign local scale to \"StartScale\""))
        {
            myScript.MakeCurrentScaleTheStartScale();
        }
    }
}
#endif


public class ChangeScale : MonoBehaviour
{

    [Tooltip("Start scale is the scale when the game starts, doesnt rely on the actualy transform!!")]
    public Vector3 StartScale = Vector3.zero;
    [Tooltip("GrowToScale is the goal scale when the public GrowToScale Function is called")]
    public Vector3 GrowToScale = Vector3.one;
    [Tooltip("Time of change, and it uses linear interpolation to get there")]
    public float TimeToChangeScale = 10;
    public AudioSource OptionalSoundToPlayOnScaleChange;

    private bool SoundToPlay;

    void Start()
    {
        transform.localScale = StartScale;
        if (OptionalSoundToPlayOnScaleChange != null)
        {

            SoundToPlay = true;
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
        transform.localScale = StartScale;
    }

    public void Grow()
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            if (SoundToPlay)
            {
                OptionalSoundToPlayOnScaleChange.Play();
            }
            StartCoroutine(ChangingScale(GrowToScale, TimeToChangeScale));
        }
    }


    public void GrowInstantly()
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            transform.localScale = GrowToScale;
        }
    }

    public void Shrink()
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(ChangingScale(StartScale, TimeToChangeScale));
        }
    }

    public void ShrinkToZeroScale()
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(ChangingScale(Vector3.zero, TimeToChangeScale));
        }
    }

    public void ShrinkToZeroScaleTimed(float duration)
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(ChangingScale(Vector3.zero, duration));
        }
    }


    IEnumerator ChangingScale(Vector3 ChangeScaleToThis, float TimeOfChange)
    {
        float elapsedTime = 0;
        while (elapsedTime < (TimeToChangeScale))
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, ChangeScaleToThis, elapsedTime / TimeOfChange);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
    public void MakeCurrentScaleTheGrowToScale()
    {
        GrowToScale = transform.localScale;
    }

    public void MakeCurrentScaleTheStartScale()
    {
        StartScale = transform.localScale;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor (typeof(TimedUnityEvents))]
public class TimedUnityEventsEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector ();

        TimedUnityEvents myScript = (TimedUnityEvents)target;

        if (GUILayout.Button ("Sort")) {
            myScript.SortEvents ();
        }
        if (GUILayout.Button ("Play")) {
            myScript.Play ();
        }


    }
}
#endif

public class TimedUnityEvents : MonoBehaviour
{

    [System.Serializable]
    public struct TimedEvents
    {
        public float timeStamp;
        public UnityEvent timedUnityEvent;
    }


    public bool PlayOnAwake;
    public bool disableWhenPlayed = false; 
    public UnityEvent firstEvent;
    public UnityEvent lastEvent;
    public TimedEvents[] timedEvents;
   
    [Header("Debuggin, Dont touch!")]
    public float currentTime;
    [HideInInspector]
    public float timePlaying = 0;
    private float startTime = 0;
    private bool isPlaying;
    private int index = 0;

    void OnEnable ()
    {
        if (PlayOnAwake) {
            Play ();
        }
    }

    public void Play ()
    {
        if (!isPlaying) {
            timePlaying = 0;
            isPlaying = true;
            startTime = Time.time;
            firstEvent.Invoke ();
        }
    }
    void Update ()
    {
        if (isPlaying) {
            
             timePlaying += Time.deltaTime;
            
            CheckAndInterate ();

            if (timePlaying >= currentTime) {
                Reset ();
            }
        } 
    }

    void OnDisable ()
    {
        Reset ();
    }

    public void Reset ()
    {
        if (isPlaying) {
            
            index = 0; 
            timePlaying = 0;
            startTime = 0;
            isPlaying = false;

            if (gameObject.activeInHierarchy) {
                lastEvent.Invoke ();
            }
            if (disableWhenPlayed)
            {
                this.gameObject.SetActive(false);
            }

        }
    }

    void CheckAndInterate ()
    {
        if (index < timedEvents.Length) {
            if (timePlaying >= timedEvents [index].timeStamp) {
                timedEvents [index].timedUnityEvent.Invoke ();
                index++;
            }
        }
    }

    public void SortEvents ()
    {
        BubbleSort ();
    }

    void BubleSortHelper (int m, int n)
    {
        TimedEvents temporary;
        temporary = timedEvents [m];
        timedEvents [m] = timedEvents [n];
        timedEvents [n] = temporary;
    }

    void BubbleSort ()
    {
        int i, j;
        int lengthOfArray = timedEvents.Length;

        for (j = lengthOfArray - 1; j > 0; j--) {
            for (i = 0; i < j; i++) {
                if (timedEvents [i].timeStamp > timedEvents [i + 1].timeStamp)
                    BubleSortHelper (i, i + 1);
            }
        }

        float max = timedEvents [0].timeStamp;

        for (i = 0; i < lengthOfArray; i++) {
            if (timedEvents [i].timeStamp > max) {
                max = timedEvents [i].timeStamp;
            }
        }

        if (currentTime < max) {
            currentTime = max;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePlayer : MonoBehaviour {

    public AudioSource audioSource; 

    public void PlayAudio(){

        audioSource.Play(); 
    }
}

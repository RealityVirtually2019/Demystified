using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{

    public void PlayParticleSystem()
    {

        this.GetComponent<ParticleSystem>().Play();
    }

    public void StopParticlesystem()
    {

        this.GetComponent<ParticleSystem>().Stop();

    }
}
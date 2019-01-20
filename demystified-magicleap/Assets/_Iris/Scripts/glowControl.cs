using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glowControl : MonoBehaviour
{

    public Transform glowSystem;
    // Use this for initialization
    void Start()
    {
        glowSystem.GetComponent<ParticleSystem>().enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        { glowSystem.GetComponent<ParticleSystem>().enableEmission = true;
            StartCoroutine(stopGlow());
        }
    }

    IEnumerator stopGlow()
    {

        yield return new WaitForSeconds(3f);
        glowSystem.GetComponent<ParticleSystem>().enableEmission = false;

    }

}

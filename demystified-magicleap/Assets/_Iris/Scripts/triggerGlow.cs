using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerGlow : MonoBehaviour
{

    public Transform glowSystem;
    private bool triggerornot=false;
    // Use this for initialization
    void Start()
    {
        glowSystem.GetComponent<ParticleSystem>().enableEmission = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("trigger entered");
        triggerornot = true;
        if (other.tag == "Player" && triggerornot ==true)
         
        glowSystem.GetComponent<ParticleSystem>().enableEmission = true;
        StartCoroutine(stopGlow());
        
    }

    IEnumerator stopGlow()
    {

        yield return new WaitForSeconds(1f);
        glowSystem.GetComponent<ParticleSystem>().enableEmission = false;
        triggerornot = false;

    }

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            glowSystem.GetComponent<ParticleSystem>().enableEmission = true;
            StartCoroutine(stopGlow());
        }
    }

    IEnumerator stopGlow()
    {

        yield return new WaitForSeconds(4f);
        glowSystem.GetComponent<ParticleSystem>().enableEmission = false;

    }*/

}

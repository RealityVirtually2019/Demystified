using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PaintingManager : ScriptableObject {

    [Header("Smoke Manager")]
    public GameObject Smoke;
    public bool hasSmokeCleared = false;
    public bool hasSmokeInstantiated = false;
    [Header("Dress")]
    public bool hasDressInstantiated = false; 
    public GameObject Dress;
   // [Header("Carnival")]

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostUpdatedImageLocation : MonoBehaviour {

    public static MostUpdatedImageLocation instance;

    public bool paintingDetected = false; 
    public Vector3 currentPaintingPosition;
    public Quaternion currentPaintingRotation;
    public GameObject ShowOnDetection; 
	public void Awake()
	{
        //Check if instance already exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) { Destroy(gameObject); }   

	}

	private void Update()
	{
        if(paintingDetected){
            this.transform.position = Vector3.Lerp(this.transform.position, this.currentPaintingPosition, Time.deltaTime * 2);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.currentPaintingRotation, Time.deltaTime * 2);
        }
	}


}

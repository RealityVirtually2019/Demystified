using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePositionLocationOfImage : MonoBehaviour {


	public void OnEnable()
	{
        MostUpdatedImageLocation.instance.paintingDetected = true; 
        MostUpdatedImageLocation.instance.currentPaintingPosition = this.transform.position;
        MostUpdatedImageLocation.instance.currentPaintingRotation = this.transform.rotation;
        MostUpdatedImageLocation.instance.ShowOnDetection.SetActive(true); 
    }
}

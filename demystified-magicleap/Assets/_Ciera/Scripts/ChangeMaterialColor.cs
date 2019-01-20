using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour {

    public Material materialToChange;
    public string PropertyToChange = "_TintColor";
    public Gradient colorGradient;
    float colorValue;
    public float time;
    // Use this for initialization
    void Awake () {

        materialToChange.EnableKeyword(PropertyToChange);

    }
    public void StartChangeColor(){
        materialToChange.EnableKeyword(PropertyToChange);
        StartCoroutine(ChangeColor());
    }
	private void OnEnable()
	{
        materialToChange.SetColor(PropertyToChange,colorGradient.Evaluate(0));
		
	}
	private void OnApplicationQuit()
	{
        materialToChange.SetColor(PropertyToChange,colorGradient.Evaluate(0));
	}
    private void OnDisable(){
        materialToChange.SetColor(PropertyToChange,colorGradient.Evaluate(0));

    }
    IEnumerator ChangeColor()
    {
        float elapsedTime = 0;
        while (elapsedTime < (time))
        {
            Debug.Log("fading");
            elapsedTime += Time.deltaTime;
            materialToChange.SetColor(PropertyToChange, colorGradient.Evaluate(elapsedTime / time));

          //  SpriteToChange.color = Color.Lerp(SpriteToChange.color, ColorToChangeTo, elapsedTime /time);
            yield return null;
        }
        yield return null;

    }
	//utilites lerp color from gradient, need : min, max, gradient, colortoChange
}

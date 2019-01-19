using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ChangeSpriteRenderer))]
public class ChangeSpriteRendererColor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChangeSpriteRenderer myScript = (ChangeSpriteRenderer)target;

        if (GUILayout.Button("Change Color Of Sprite To Clear"))
        {
            myScript.ChangeColorOfSpriteToClear();

        }
        if (GUILayout.Button("Change Color Of Sprite Back To Original Color"))
        {
            myScript.ChangeColorOfSpriteBackToOriginalColor();
        }
        if (GUILayout.Button("Change Color Of Sprite"))
        {
            myScript.ChangeColorOfSprite();
        }



    }
}
#endif
public class ChangeSpriteRenderer : MonoBehaviour
{
    public float timeOfColorTransition = 10f;
    public Color ColorToChange;
    Color StartColor;
    SpriteRenderer SpriteToChange;

    void Awake()
    {
        SpriteToChange = GetComponent<SpriteRenderer>();
        StartColor = SpriteToChange.color;

    }

    IEnumerator ChangingColorOfSprite(Color ColorToChangeTo)
    {
        float elapsedTime = 0;
        while (elapsedTime < (timeOfColorTransition))
        {
            elapsedTime += Time.deltaTime;
            SpriteToChange.color = Color.Lerp(SpriteToChange.color, ColorToChangeTo, elapsedTime / timeOfColorTransition);
            yield return null;
        }
        yield return null;

    }
    public void ChangeColorOfSpriteInstantly()
    {

        StopAllCoroutines();
        SpriteToChange.color = ColorToChange;

    }

    public void RevertColorOfSpriteInstantly()
    {
        StopAllCoroutines();
        SpriteToChange.color = StartColor;
    }
    public void ChangeColorOfSpriteToClear()
    {

        StopAllCoroutines();
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ChangingColorOfSprite(Color.clear));
        }
    }

    public void ChangeColorOfSpriteBackToOriginalColor()
    {

        StopAllCoroutines();
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ChangingColorOfSprite(StartColor));
        }
    }

    public void ChangeColorOfSprite()
    {
        SpriteToChange = GetComponent<SpriteRenderer>();
        StopAllCoroutines();
        StartCoroutine(ChangingColorOfSprite(ColorToChange));

    }
    void OnDisable()
    {

        StopAllCoroutines();
        SpriteToChange.color = StartColor;

    }
}

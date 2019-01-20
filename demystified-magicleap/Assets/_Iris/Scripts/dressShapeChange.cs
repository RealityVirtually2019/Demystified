using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dressShapeChange : MonoBehaviour {

    private float mSize = 0.0f;

    private SkinnedMeshRenderer rd;
   
    //make the dress morph between state1 and state2 (drag shapekey1)

    void Start () {

        rd = GetComponent<SkinnedMeshRenderer>();
        InvokeRepeating("state1", 0.0f, 0.01f);
    }
	
	void state1 () {
        if (mSize >= 150f)
        {
            CancelInvoke("state1");
            InvokeRepeating("state2", 0.0f, 0.01f);
        }
        rd.SetBlendShapeWeight(0, mSize++);
    }

    void state2()
    {
        if (mSize <=0f)
        {
            CancelInvoke("state2");
            InvokeRepeating("state1", 0.0f, 0.01f);
        }
        rd.SetBlendShapeWeight(0, mSize--);
    }




    //how to finish Shapekey1 and then wait a few seconds and finish Shapekey2

    /*
         private float mSize = 0.0f;
    private float mSiize = 0.0f;
    private SkinnedMeshRenderer rd;
    // Use this for initialization

    void Start () {

        rd = GetComponent<SkinnedMeshRenderer>();
        InvokeRepeating("Scale", 0.0f, 0.01f);
        StartCoroutine(Scaling());

    }

    // Update is called once per frame
    void Scale () {
        if (mSize >= 100f)
        {
            CancelInvoke("Scale");
        }
        rd.SetBlendShapeWeight(0, mSize++);
    }


    IEnumerator Scaling()
    {
        yield return new WaitForSeconds(1f);
        InvokeRepeating("Scale2", 0.0f, 0.01f);
    }

    void Scale2()
    {
        if (mSiize >= 200f)
        {
            CancelInvoke("Scale2");
        }
        rd.SetBlendShapeWeight(1, mSiize++);
    }

        */
}

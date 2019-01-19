using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotate = Vector3.right;
    public float scalar = .1f;
    void Update()
    {
        // Rotate the object around its local X axis at 1 degree per second
        transform.Rotate(rotate * scalar * Time.deltaTime);
        
    }
}
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 axis;
    public float speed;

    void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}

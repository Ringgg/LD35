using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    [SerializeField]
    float destroyTime = 1.0f;

    void Start()
    {
        Invoke("Destroy", destroyTime);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
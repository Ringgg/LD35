using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject foodPrefab;
    public float spawnDelay = 3.0f;

    void Start()
    {
        InvokeRepeating("Spawn", spawnDelay, spawnDelay);
    }

    void Spawn()
    {
        float x = Random.Range(-18.0f, 18.0f);
        float z = Random.Range(-18.0f, 18.0f);
        Instantiate(foodPrefab, new Vector3(x, 8.0f, z), Quaternion.identity);
    }
}

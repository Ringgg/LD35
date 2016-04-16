using UnityEngine;

public class AICitizen : MonoBehaviour
{
    Vector3 lastKnownLocation;
    float locationUpdateTme;

    void Start()
    {
        Game.instance.citizens.Add(this);
    }

    void OnDestroy()
    {
        Game.instance.citizens.Remove(this);
    }
}

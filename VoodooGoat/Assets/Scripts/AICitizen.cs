using UnityEngine;

public class AICitizen : MonoBehaviour
{
    public Vector3 lastKnownLocation;
    public float locationUpdateTme;

    void Start()
    {
        Game.instance.citizens.Add(this);
    }

    void OnDestroy()
    {
        if (Game.instance != null)
            Game.instance.citizens.Remove(this);
    }
}

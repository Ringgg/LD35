using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State { dude, goat }
    public State state;

    public bool compromised;

    void Update()
    {

    }

    public bool IsVisible()
    {
        return false;
    }
}

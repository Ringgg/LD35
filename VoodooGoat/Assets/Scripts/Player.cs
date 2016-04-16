using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State { dude, goat }
    public State state;

    public bool compromised = true;

    void Start()
    {
        Game.instance.player = this;
    }

    void Update()
    {

    }

    public bool IsVisible()
    {
        return false;
    }
}

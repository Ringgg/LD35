using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State { dude, goat }
    public State state;

    [SerializeField]
    GameObject SmokePrefab;
    public bool compromised = true;

    void Start()
    {
        Game.instance.player = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (SmokePrefab != null)
                Instantiate(SmokePrefab, transform.position + Vector3.up, Quaternion.identity);

            if(state == State.goat)
            {
                if (IsVisible())
                    compromised = true;
                state = State.dude;
            }
            else
                compromised = IsVisible();
            {
                state = State.goat;
            }
        }
    }


    public bool IsVisible()
    {
        foreach (AIPoliceman p in Game.instance.policemen)
            if (Vision.CanSee(p.transform, transform, p.sightRange))
                return true;

        foreach (AICitizen c in Game.instance.citizens)
            if (Vision.CanSee(c.transform, transform, c.sightRange))
                return true;

        return false;
    }
}

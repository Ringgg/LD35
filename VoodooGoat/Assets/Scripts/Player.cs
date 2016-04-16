using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State { dude, goat }
    public State state;

    [SerializeField]
    GameObject SmokePrefab;
    public bool compromised = true;
    public bool isGoatForFifeSeconds = true;
    public float shiftCooldown;

    void Start()
    {
        Game.instance.player = this;
    }

    void Update()
    {
        shiftCooldown = Mathf.MoveTowards(shiftCooldown, 0.0f, Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (shiftCooldown != 0)
            {
                //negative feedback
                return;
            }

            if (SmokePrefab != null)
                Instantiate(SmokePrefab, transform.position + Vector3.up, Quaternion.identity);

            if(state == State.goat)
            {
                if (IsVisible())
                    compromised = true;
                state = State.dude;
            }
            else
            {
                compromised = IsVisible();
                state = State.goat;
            }
        }
    }
    

    public void ComboBreaker()
    {
        //player was seen as a goat by police shorter than 5 seconds after being seen
        compromised = true;
        shiftCooldown += 3.0f;
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

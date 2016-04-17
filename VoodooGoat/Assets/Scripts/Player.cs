using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public enum State { dude, goat }
    public State state;

    [SerializeField] GameObject SmokePrefab;
    [SerializeField] GameObject dudeMesh;
    [SerializeField] GameObject goatMesh;

    public bool compromised = true;
    public bool wasGoatForFifeSeconds = true;
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

            shiftCooldown += 1.0f;

            if (SmokePrefab != null)
                Instantiate(SmokePrefab, transform.position, Quaternion.identity);

            if(state == State.goat)
            {
                if (IsVisible())
                    compromised = true;
                state = State.dude;
                dudeMesh.SetActive(true);
                goatMesh.SetActive(false);
            }
            else
            {
                ChangeToGoat();
            }
        }
    }
    
    IEnumerator AfterChangingToGoat()
    {
        float timePassed = 0.0f;
        wasGoatForFifeSeconds = false;
        //start timer

        while(timePassed < 5.0f)
        {
            if (IsVisibleByPoliceman())
            {
                //todo: spawn particle
                //play sound
                compromised = true;
                shiftCooldown += 3.0f;
                yield break;
            }
            timePassed += Time.deltaTime;
            yield return null;
        }

        //end timer
        wasGoatForFifeSeconds = true;
    }


    public void ChangeToGoat()
    {
        if (!IsVisible())
        {
            if (compromised)
            {
                compromised = false;
                StartCoroutine("AfterChangingToGoat");
            }
        }
        else
            compromised = true;

        state = State.goat;
        dudeMesh.SetActive(false);
        goatMesh.SetActive(true);
    }


    public void GetCatchedPoliceman()
    {
        if (!Game.instance.finished)
        {
            Game.instance.finished = true;
            Debug.Log("you got catched by a policeman");
            GetComponent<ThirdPersonUserControl>().enabled = false;
            enabled = false;
        }
    }


    public void GetCatchedTerririst()
    {
        if (!Game.instance.finished)
        {
            Game.instance.finished = true;
            Debug.Log("you got catched by a terrorist");
            GetComponent<ThirdPersonUserControl>().enabled = false;
            enabled = false;
        }
    }


    public void RunOutOfTime()
    {
        if (!Game.instance.finished)
        {
            Game.instance.finished = true;
            Debug.Log("you ran out of time");
            GetComponent<ThirdPersonUserControl>().enabled = false;
            enabled = false;
        }
    }


    public void WinGame()
    {
        if (!Game.instance.finished)
        {
            Game.instance.finished = true;
            Debug.Log("you won");
            GetComponent<ThirdPersonUserControl>().enabled = false;
            enabled = false;
        }
    }


    public bool IsVisibleByPoliceman()
    {
        foreach (AIPoliceman p in Game.instance.policemen)
            if (Vision.CanSee(p.transform, transform, p.sightRange))
                return true;
        return false;
    }

    public bool IsVisible()
    {
        foreach (AIPoliceman p in Game.instance.policemen)
            if (Vision.CanSee(p.transform, transform, p.sightRange))
                return true;

        foreach (AICitizen c in Game.instance.citizens)
            if (Vision.CanSee(c.transform, transform, c.sightRange))
                return true;

        foreach (AITerrorist t in Game.instance.terrorists)
            if (Vision.CanSee(t.transform, transform, t.sightRange))
                return true;

        return false;
    }
}

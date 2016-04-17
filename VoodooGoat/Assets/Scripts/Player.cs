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

            shiftCooldown += 1.0f;
            isGoatForFifeSeconds = false;

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
                compromised = IsVisible();
                state = State.goat;
                dudeMesh.SetActive(false);
                goatMesh.SetActive(true);
            }
        }
    }
    
    IEnumerator AfterChangingToGoat()
    {
        float timePassed = 0.0f;

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
        isGoatForFifeSeconds = true;
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

        return false;
    }
}

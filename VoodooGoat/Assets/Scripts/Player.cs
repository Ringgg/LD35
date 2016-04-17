using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    public enum State { dude, goat }
    public State state;

    public GameObject endGoatTerro;
    public GameObject endGoatPolice;
    public GameObject endDudePolice;
    public GameObject endEndTime;
    public GameObject endVictory;
    public Action OnGameEnd;

    [SerializeField] GameObject SmokePrefab;
    [SerializeField] GameObject dudeMesh;
    [SerializeField] GameObject goatMesh;
    [SerializeField] AudioClip poof;
    [SerializeField] AudioClip[] goat;

    public bool compromised = true;
    public bool wasGoatForFifeSeconds = true;
    public bool visible = false;
    public float shiftCooldown;

    AudioSource audio;
    public int remainingShiftsBack = 3;

    void Start()
    {
        Game.instance.player = this;
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        shiftCooldown = Mathf.MoveTowards(shiftCooldown, 0.0f, Time.deltaTime);

        visible = IsVisible();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (shiftCooldown != 0)
            {
                //negative feedback
                return;
            }

            shiftCooldown += 1.0f;

            audio.clip = poof;
            audio.Play();
            
            if(state == State.goat)
            {
                if (remainingShiftsBack == 0) return;

                remainingShiftsBack--;
                if (SmokePrefab != null)
                    Instantiate(SmokePrefab, transform.position, Quaternion.identity);
                if (visible)
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
        if (SmokePrefab != null)
            Instantiate(SmokePrefab, transform.position, Quaternion.identity);

        if (!visible)
        {
            if (compromised)
            {
                compromised = false;
                StartCoroutine("AfterChangingToGoat");
            }
        }
        else
            compromised = true;

        audio.clip = goat[UnityEngine.Random.Range(0, goat.Length - 1)];
        audio.Play();
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
            if (OnGameEnd != null)
                OnGameEnd();
            if (state == State.goat)
                endGoatPolice.SetActive(true);
            else
                endDudePolice.SetActive(true);
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
            if (OnGameEnd != null)
                OnGameEnd();
            endGoatTerro.SetActive(true);
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
            if (OnGameEnd != null)
                OnGameEnd();
            endEndTime.SetActive(true);
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
            if (OnGameEnd != null)
                OnGameEnd();
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

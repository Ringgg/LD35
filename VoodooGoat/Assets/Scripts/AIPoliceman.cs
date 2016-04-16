using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AICharacterControl))]
public class AIPoliceman : MonoBehaviour
{
    AICharacterControl controller;
    
    Vector3 lastKnownLocation;
    float locationUpdateTime = -1.0f;
    public float sightRange = 10.0f;
    float talkRange = 3.0f;
    float talkTime = 2.0f;

    //helper variables
    RaycastHit hitInfo;
    Player player;
    AICitizen citizen;

    void Start()
    {
        Game.instance.policemen.Add(this);
        player = Game.instance.player;
        controller = GetComponent<AICharacterControl>();

        StartCoroutine("Wander");
    }

    void Update()
    {
        if (citizen != null)
            Debug.DrawLine(transform.position, citizen.transform.position);
        if (controller.target != null)
            Debug.DrawLine(transform.position, controller.target.transform.position);
    }
    
    void OnDestroy()
    {
        if (Game.instance != null)
            Game.instance.policemen.Remove(this);
    }

    IEnumerator Chase()
    {
        controller.target = player.transform;

        while (CanChase())
        {
            lastKnownLocation = player.transform.position;
            yield return null;
        }

        controller.target = null;
        StartCoroutine("CatchUp");
    }


    IEnumerator CatchUp()
    {
        controller.agent.SetDestination(lastKnownLocation);

        yield return null;
        Debug.Log(controller.agent.remainingDistance);
        while (!IsAtDeadEnd())
        {
            if (CanChase())
            {
                StartCoroutine("Chase");
                yield break;
            }

            yield return null;
        }
        Debug.Log("GoingToCitizen");
        GetNearestVisibleCitizen();
        if (citizen != null)
        {
            StartCoroutine("Investigate");
            yield break;
        }
        else
        {
            StartCoroutine("Wander");
            yield break;
        }
    }


    IEnumerator Wander()
    {
        while (!CanChase())
        {
            yield return null;
        }

        controller.target = player.transform;
        StartCoroutine("Chase");
        yield break;
    }


    IEnumerator Investigate()
    {
        if (citizen == null)
            yield break;

        float remainingTime = talkTime;

        while (Vector3.Distance(transform.position, citizen.transform.position) > talkRange)
        {
            controller.target = citizen.transform;
            if (CanChase())
            {
                citizen = null;
                StartCoroutine("Chase");
                yield break;
            }

            yield return null;
        }

        controller.target = null;
        while (remainingTime > 0.0f)
        {
            remainingTime -= Time.deltaTime;
            if (CanChase())
            {
                citizen = null;
                StartCoroutine("Chase");
                yield break;
            }

            yield return null;
        }

        if (citizen.locationUpdateTime > locationUpdateTime)
        {
            lastKnownLocation = citizen.lastKnownLocation;
            locationUpdateTime = citizen.locationUpdateTime;
            StartCoroutine("CatchUp");
            yield break;
        }
        else
        {
            StartCoroutine("Wander");
            yield break;
        }
    }


    bool CanChase()
    {
        return Vision.CanSee(transform, player.transform, sightRange) && player.compromised;
    }


    bool IsAtDeadEnd()
    {
        return controller.agent.stoppingDistance > controller.agent.remainingDistance;
    }


    void GetNearestVisibleCitizen()
    {
        citizen = null;
        float closestDist = float.MaxValue;
        float dist;
        foreach(AICitizen c in Game.instance.citizens)
        {
            dist = Vector3.Distance(transform.position, c.transform.position);
            if (dist < closestDist && Vision.CanSee(transform, c.transform, sightRange) && c.locationUpdateTime > locationUpdateTime)
            {
                citizen = c;
            }
        }
    }
}

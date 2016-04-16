using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AICharacterControl))]
public class AIPoliceman : MonoBehaviour
{
    AICharacterControl controller;
    
    Vector3 lastKnownLocation;
    float locationUpdateTme = -1.0f;
    float sightRange = 10.0f;
    float talkRange = 4.0f;
    float talkTime = 1.0f;
    bool pursuing = true;

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
        if (pursuing)
        {
            if (player.compromised && CanSee(player.transform))
            {
                pursuing = true;
                controller.target = player.transform;
                lastKnownLocation = player.transform.position;
            }
            else
            {
                controller.target = null;
                if (!IsAtDeadEnd())
                {
                    controller.agent.SetDestination(lastKnownLocation);
                }
                else
                {
                    citizen = GetNearestVisibleCitizen();
                }
            }
        }
        //else wander around
    }


    void OnDestroy()
    {
        if (Game.instance != null)
            Game.instance.policemen.Remove(this);
    }

    IEnumerator Chase()
    {
        controller.target = player.transform;

        while (player.compromised && CanSee(player.transform))
        {
            yield return null;
        }

        controller.target = null;
        StartCoroutine("CatchUp");
    }

    IEnumerator CatchUp()
    {
        controller.agent.SetDestination(lastKnownLocation);
        while(controller.agent.remainingDistance > controller.agent.stoppingDistance)
        {
            //checksight -> chase

            yield return null;
        }

        citizen = GetNearestVisibleCitizen();
        StartCoroutine("Investigate");
    }

    IEnumerator Wander()
    {
        
        yield return null;
    }

    IEnumerator Investigate()
    {
        if (citizen != null)
        {
            float remainingTime = talkTime;

            while (Vector3.Distance(transform.position, citizen.transform.position) < talkRange)
            {
                controller.target = citizen.transform;

                yield return null;
            }
            
            while (remainingTime > 0.0f)
            {
                remainingTime -= Time.deltaTime;
                
                yield return null;
            }

            if (citizen.locationUpdateTme > locationUpdateTme)
            {
                lastKnownLocation = citizen.lastKnownLocation;
                StartCoroutine("Chase");
            }
            else
            {
                StartCoroutine("Wander");
            }
            citizen = null;
        }
    }


    bool CanSee(Transform t)
    {
        if (!Physics.Raycast(new Ray(transform.position, player.transform.position - transform.position),
            out hitInfo, 
            sightRange))
            return false;

        return hitInfo.collider.transform == player;
    }


    bool IsAtDeadEnd()
    {
        return controller.agent.stoppingDistance > controller.agent.remainingDistance;
    }


    AICitizen GetNearestVisibleCitizen()
    {
        return null;
    }
}

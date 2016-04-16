using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AICharacterControl))]
public class AIPoliceman : MonoBehaviour
{
    AICharacterControl controller;
    
    public Vector3 lastKnownLocation;
    public float locationUpdateTime = -1.0f;
    public float sightRange = 10.0f;
    float talkRange = 3.0f;
    float talkTime = 2.0f;
    float x = 0.5f;
    public Transform[] targetList;

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

        while (!IsAtDeadEnd())
        {
            if (CanChase())
            {
                StartCoroutine("Chase");
                yield break;
            }

            yield return null;
        }
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
        yield return new WaitForSeconds(2.0f);
        SetNewTarget();
        while (!CanChase())
        {
            if (controller.agent.remainingDistance < x)
            {
                SetNewTarget();
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }

        controller.target = player.transform;
        StartCoroutine("Chase");
        yield break;
    }

    IEnumerator Follow()
    {
        while (true)
        {

            yield return new WaitForEndOfFrame();
            //yield return null;
        }
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
            //todo: play talking sound
            //place indicator of position a citizen is pointing
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

    

    void SetNewTarget()
    {
        do
        {
            controller.target = targetList[Random.Range(0, targetList.Length - 1)];

        } while (controller.target.position == lastKnownLocation);
        lastKnownLocation = controller.target.position;
    }
}

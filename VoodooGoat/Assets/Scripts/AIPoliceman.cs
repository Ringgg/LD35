using UnityEngine;

[RequireComponent (typeof(AICharacterControl))]
public class AIPoliceman : MonoBehaviour
{
    AICharacterControl controller;

    Transform pointer;

    Vector3 lastKnownLocation;
    float locationUpdateTme;

    void Start()
    {
        Game.instance.policemen.Add(this);
        controller = GetComponent<AICharacterControl>();
    }


    void Update()
    {

    }


    void OnDestroy()
    {
        Game.instance.policemen.Remove(this);
    }


    bool CanSee(Transform t)
    {
        return false;
    }


    bool IsCompromised(Player p)
    {
        return p.compromised;
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

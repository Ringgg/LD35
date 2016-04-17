using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AICharacterControl))]
public class AITerrorist : MonoBehaviour {

    public Vector3 lastKnownLocation;
    public Vector3 startLocation;
    public float sightRange = 10.0f;
    public float catchRange = 1.0f;

    [SerializeField]
    ParticleSystem particleEffect;

    Player player;

    AICharacterControl controller;

    void Start()
    {
        Game.instance.terrorists.Add(this);
        player = Game.instance.player;
        controller = GetComponent<AICharacterControl>();
        lastKnownLocation = transform.position;
        startLocation = transform.position;
        StartCoroutine("Stand");
    }


    IEnumerator Stand()
    {
        particleEffect.enableEmission = false;
        yield return new WaitForSeconds(2.0f);
        controller.agent.destination = startLocation;

        while (!CanChase())
        {
            yield return null;
        }

        controller.target = player.transform;
        StartCoroutine("Chase");
        yield break;
    }


    IEnumerator Chase()
    {
        controller.target = player.transform;

        while (CanChase())
        {
            if (Vector3.Distance(transform.position, player.transform.position) < catchRange)
            {
                player.GetCatchedTerririst();
                yield break;
            }
            lastKnownLocation = player.transform.position;
            particleEffect.enableEmission = true;
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

        StartCoroutine("Stand");
        yield break;
    }


    bool IsAtDeadEnd()
    {
        return controller.agent.stoppingDistance > controller.agent.remainingDistance;
    }


    bool CanChase()
    {
        return Vision.CanSee(transform, player.transform, sightRange) && player.state == Player.State.goat;
    }


    void OnDestroy()
    {
        if (Game.instance != null)
            Game.instance.terrorists.Remove(this);
    }


}

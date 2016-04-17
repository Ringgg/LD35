using UnityEngine;
using System.Collections;

public class Ritual : MonoBehaviour
{

    bool done = false;
    bool stop = false;
    public float time;

    [SerializeField]
    GameObject particleEffect;

    void OnTriggerEnter(Collider other)
    {
        stop = false;
        if (other.gameObject.tag == "Player" && !done && Game.instance.player.state == Player.State.dude)
            StartCoroutine(DoRitual());
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            StopCoroutine(DoRitual());
        stop = true;
    }

    IEnumerator DoRitual()
    {
        float timePassed = 0;

        while (timePassed < time && !stop)
        {
            timePassed++;
            Debug.Log(timePassed);
            yield return new WaitForSeconds(1);
        }
        if (timePassed >= time)
        {
            done = true;
            Finish();
        }
    }

    void Finish()
    {
        Game.instance.player.compromised = true;

        //najblizszy policjant idzie na miejsce zdarzenia
        AIPoliceman closest = null;
        float closestDist = float.MaxValue;
        float currentDist;
        foreach (AIPoliceman p in Game.instance.policemen)
        {
            currentDist = Vector3.Distance(transform.position, p.transform.position);
            if (currentDist < closestDist)
            {
                closestDist = currentDist;
                closest = p;
            }
        }

        closest.lastKnownLocation = transform.position;
        closest.locationUpdateTime = Time.time;

        //przez 10s nie mozesz sie zmienic
        Game.instance.player.shiftCooldown = 10.0f;

        //spawn particle
        if (particleEffect != null)
            Instantiate(particleEffect, transform.position + Vector3.up, Quaternion.identity);
    }
}

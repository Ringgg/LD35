using UnityEngine;
using System.Collections;

public class Ritual : MonoBehaviour
{

    bool done = false;
    bool stop = false;
    public float time;

    void OnTriggerEnter(Collider other)
    {
        stop = false;
        if (other.gameObject.tag == "Player" && !done)
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
        //przez 10s nie mozesz sie zmienic
        //spawn particle
    }
}

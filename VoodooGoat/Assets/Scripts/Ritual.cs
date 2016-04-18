using UnityEngine;
using System.Collections;

public class Ritual : MonoBehaviour
{
    bool done = false;
    bool stop = false;
    public float time;

    [SerializeField]
    ParticleSystem particleEffect;
    [SerializeField]
    Material finishedMat;
    AudioSource audio;

    void Start()
    {
        Game.instance.ritualsRemaining++;
        audio = GetComponent<AudioSource>();
    }


    void OnTriggerEnter(Collider other)
    {
        stop = false;
        if (other.gameObject.tag == "Player" && !done && Game.instance.player.state == Player.State.dude && !Game.instance.player.compromised)
        {
            StopCoroutine(DoRitual());
            StartCoroutine(DoRitual());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopCoroutine(DoRitual());
            particleEffect.enableEmission = false;
            audio.Stop();
        }
        stop = true;
    }

    IEnumerator DoRitual()
    {
        particleEffect.enableEmission = true;
        float timePassed = 0;
        audio.Play();
        while (timePassed < time && !stop)
        {
            timePassed = Mathf.Clamp(timePassed + Time.deltaTime, 0, float.MaxValue);
            Debug.Log(timePassed);
            yield return new WaitForSeconds(0);
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
        audio.Stop();
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
        
        closest.ForceCatchUp(transform.position);

        //przez 5s nie mozesz sie zmienic
        Game.instance.player.shiftCooldown = 5.0f;

        //spawn particle
        particleEffect.enableEmission = false;
        GetComponent<MeshRenderer>().material = finishedMat;

        //game logic
        Game.instance.ritualsRemaining--;
        Game.instance.timeRemaining += 20.0f;
        //Game.instance.player.ChangeToGoat();
        Debug.Log("Ritual completed. " + Game.instance.ritualsRemaining + " more remaining");
        if (Game.instance.ritualsRemaining == 0)
            Game.instance.player.WinGame();
    }
}

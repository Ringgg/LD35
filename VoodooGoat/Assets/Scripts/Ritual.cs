using UnityEngine;
using System.Collections;

public class Ritual : MonoBehaviour {

    bool done;
    float time;

    void OnTriggerStay(Collider other)
    {

    }

    IEnumerator DoRitual()
    {
        yield return new WaitForSeconds(1);

        float timePassed = 0;

        while (false)
        {

        }
    }
}

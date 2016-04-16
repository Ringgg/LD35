using UnityEngine;
using System.Collections;

public class CitizenTarget : MonoBehaviour {

    AITerrorist terrorist;
    float waitTime =5;


    void OnTriggerEnter(Collider other)
    {
        terrorist = other.GetComponent<AITerrorist>();
        if(terrorist != null)
        {
            StartCoroutine(Stay());
        }
    }

    IEnumerator Stay()
    {
        yield return new WaitForSeconds(waitTime);

        terrorist.lastKnownLocation = transform.position;
        terrorist.isMoving = false;
        terrorist.SetNewTarget();
    }
}

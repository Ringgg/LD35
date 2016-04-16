using UnityEngine;
using System.Collections;

public class CitizenTarget : MonoBehaviour {

    AICitizen citizen;
    float waitTime =5;


    void OnTriggerEnter(Collider other)
    {
        citizen = other.GetComponent<AICitizen>();
        if(citizen != null)
        {
            StartCoroutine(Stay());
        }
    }

    void OnTriggerExit(Collider other)
    {
        citizen = null;
    }

    IEnumerator Stay()
    {
        yield return new WaitForSeconds(waitTime);

        citizen.lastKnownLocation = transform.position;
        citizen.isMoving = false;
        citizen.SetNewTarget();
    }
}

using UnityEngine;
using System.Collections;

public class AICitizen : MonoBehaviour
{
    public Vector3 lastKnownLocation;
    float locationUpdateTme;
    public bool isMoving = true;

    AICharacterControl controller;

    public Transform[] targetList;

    void Start()
    {
        controller = GetComponent<AICharacterControl>();
        lastKnownLocation = transform.position;
        controller.target = targetList[Random.Range(0, targetList.Length - 1)];
    }

    public void SetNewTarget()
    {
        if (!isMoving)
        {
            do
            {
                controller.target = targetList[Random.Range(0, targetList.Length - 1)];
            } while (controller.target.position == lastKnownLocation);

            isMoving = true;
        }
    }
}

using UnityEngine;
using System.Collections;

public class AICitizen : MonoBehaviour
{
    public Vector3 lastKnownLocation;
    public float locationUpdateTme;
    public bool isMoving = true;

    AICharacterControl controller;

    public Transform[] targetList;

    void Start()
    {
        controller = GetComponent<AICharacterControl>();
        lastKnownLocation = transform.position;
        Game.instance.citizens.Add(this);
    }


    void OnDestroy()
    {
        if (Game.instance != null)
            Game.instance.citizens.Remove(this);
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

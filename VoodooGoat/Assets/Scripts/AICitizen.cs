using UnityEngine;
using System.Collections;

public class AICitizen : MonoBehaviour
{
    public Vector3 lastKnownLocation;
    public float locationUpdateTime;
    public float sightRange = 10.0f;
    public bool isMoving = true;

    AICharacterControl controller;

    public Transform[] targetList;

    //helper variables
    RaycastHit hitInfo;

    void Start()
    {
        controller = GetComponent<AICharacterControl>();
        lastKnownLocation = transform.position;
        Game.instance.citizens.Add(this);
    }


    void Update()
    {
        if (Game.instance.player.compromised && Vision.CanSee(transform, Game.instance.player.transform, sightRange))
        {
            lastKnownLocation = Game.instance.player.transform.position;
            locationUpdateTime = Time.time;
        }
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

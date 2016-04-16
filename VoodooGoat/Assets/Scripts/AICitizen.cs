using UnityEngine;
using System.Collections;

public class AICitizen : MonoBehaviour
{
    public Vector3 lastKnownLocation;
    public float locationUpdateTime;
    public float sightRange = 10.0f;
    public bool isMoving = true;
    GameObject instance;

    public GameObject warning;

    AICharacterControl controller;

    public Transform[] targetList;

    //helper variables
    RaycastHit hitInfo;

    void Start()
    {
        controller = GetComponent<AICharacterControl>();
        lastKnownLocation = transform.position;
        Game.instance.citizens.Add(this);
        instance = Instantiate(warning) as GameObject;
        instance.transform.position = transform.position + new Vector3(0, 1, 0);
        instance.active = false;
    }


    void Update()
    {
        if (Game.instance.player.compromised && Vision.CanSee(transform, Game.instance.player.transform, sightRange))
        {
            if (!instance.active)
            {
                instance.active = true;
            }
            lastKnownLocation = Game.instance.player.transform.position;
            locationUpdateTime = Time.time;
        }
        else
        {
            instance.active = false;
        }
    }

    void OnDestroy()
    {
        if (Game.instance != null)
            Game.instance.citizens.Remove(this);
    }
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AICharacterControl))]
public class AITerrorist : MonoBehaviour {

    public Vector3 lastKnownLocation;
    public float locationUpdateTme;
    public bool isMoving = true;
    bool seeGoat = false;
    float sightRange = 10.0f;

    RaycastHit hitInfo;
    Player player;

    AICharacterControl controller;

    public Transform[] targetList;

    void Start()
    {
        Game.instance.terrorists.Add(this);
        player = Game.instance.player;
        controller = GetComponent<AICharacterControl>();
        lastKnownLocation = transform.position;

        controller.target = targetList[Random.Range(0, targetList.Length - 1)];
    }

    void Update()
    {
        //Debug.Log(Game.instance.player.transform.position);
        if (Vision.CanSee(transform, player.transform, sightRange))
        {
            Debug.Log("See you");
        }
    }

    void OnDestroy()
    {
        if (Game.instance != null)
            Game.instance.terrorists.Remove(this);
    }


}

﻿using UnityEngine;
using System.Collections;

public class AICitizen : MonoBehaviour
{
    public Vector3 lastKnownLocation;
    public float locationUpdateTime;
    public float sightRange = 10.0f;
    public bool isMoving = true;
    [SerializeField] ParticleSystem warning;

    AICharacterControl controller;

    //public Transform[] targetList;

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
            if (!warning.enableEmission)
            {
                warning.enableEmission = true;
                //instance.active = true;
            }
            warning.enableEmission = true;
            lastKnownLocation = Game.instance.player.transform.position;
            locationUpdateTime = Time.time;
        }
        else
        {
            //instance.active = false;
            warning.enableEmission = false;
        }
    }

    void OnDestroy()
    {
        if (Game.instance != null)
            Game.instance.citizens.Remove(this);
    }
}

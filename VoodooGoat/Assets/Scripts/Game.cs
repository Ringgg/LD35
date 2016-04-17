using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    static Game _instance;
    static bool quitting = false;

    public Player player;
    public List<AICitizen> citizens = new List<AICitizen>();
    public List<AIPoliceman> policemen = new List<AIPoliceman>();
    public List<AITerrorist> terrorists = new List<AITerrorist>();

    public int ritualsRemaining;
    public float timeRemaining;

    public bool finished = false;

    void Start()
    {
        timeRemaining = 120.0f;
    }


    void Update()
    {
        timeRemaining -= Mathf.Clamp(timeRemaining - Time.deltaTime, 0, float.MaxValue);
        if (timeRemaining == 0.0f)
            player.RunOutOfTime();
    }


    void OnApplicationQuit()
    {
        quitting = true;
    }


    public static Game instance
    {
        get
        {
            if (_instance == null && !quitting)
                _instance = new GameObject("Game").AddComponent<Game>();
            return _instance;
        }
    }
}

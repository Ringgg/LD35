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

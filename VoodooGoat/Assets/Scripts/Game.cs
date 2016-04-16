using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    static Game _instance;
    
    public Player player;
    public List<AICitizen> citizens = new List<AICitizen>();
    public List<AIPoliceman> policemen = new List<AIPoliceman>();
    

    public static Game instance
    {
        get
        {
            if (_instance == null)
                _instance = new Game();
            return _instance;
        }
    }
}

﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    Player player { get { return Game.instance.player; } }

    public Text textVisibility;
    
    public Text textTimeRemaining;

    public Text textCooldown;

    public Text textCurrentTask;

    public GameObject trans1;
    public GameObject trans2;

    void Start()
    {
        Game.instance.player.OnGameEnd += EndGame;
    }

    void OnGUI()
    {
        textVisibility.text = "Visible: " + player.visible;
        
        textTimeRemaining.text = "time remaining: " + Game.instance.timeRemaining;

        textCooldown.text = "shift cooldown: " + player.shiftCooldown;

        string text = "";

        if (player.state == Player.State.goat)
        {
            if (player.remainingShiftsBack == 0)
                text += "look for food!";
            else if ( (player.wasGoatForFifeSeconds && !player.visible) || player.compromised )
                text += "shift back to human form!";
            else
                text += "Hide!";
        }
        else if (player.compromised)
            text += "Hide and shift to goat form!";
        else if (!player.wasGoatForFifeSeconds)
            text += "Hide from policemen!";
        else
            text += "Perform a ritual!";
        textCurrentTask.text = "current task: " + text;
    }


    public void EndGame()
    {
        trans1.SetActive(true);
        trans2.SetActive(true);
    }

    
    public void Exit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}

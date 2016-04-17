using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    Player player { get { return Game.instance.player; } }

    public Text textVisibility;
    public Text textSearching;
    public Text textCivils;

    public Text textRitualsRemaining;
    public Text textTimeRemaining;

    public Text textCooldown;
    public Text textCarmaRemaining;

    public Text textCurrentTask;

    void OnGUI()
    {
        textVisibility.text = "Visible: " + player.visible;
        textSearching.text = "Police recognise you: " + (player.compromised || !player.wasGoatForFifeSeconds);
        textCivils.text = "Civilians recognise you: " + player.compromised;

        textRitualsRemaining.text = "rituals to perform: " + Game.instance.ritualsRemaining;
        textTimeRemaining.text = "time remaining: " + Game.instance.timeRemaining;

        textCooldown.text = "shift cooldown: " + player.shiftCooldown;
        textCarmaRemaining.text = "remaining shapeshifts back: " + player.remainingShiftsBack;

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
}

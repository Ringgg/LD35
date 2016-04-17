using UnityEngine;

public class Food : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player" && Game.instance.player.state == Player.State.goat)
        {
            Game.instance.player.remainingShiftsBack += 1;
            Destroy(gameObject);
        }
    }
}

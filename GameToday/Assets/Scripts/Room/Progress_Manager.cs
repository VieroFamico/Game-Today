using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress_Manager : MonoBehaviour
{
    public Collider2D victoryCollider;

    public Dialog victoryDialog;

    private bool isVictory = false;
    private void Update()
    {
        if (!isVictory)
        {
            CheckForVictoryCollider();
        }
    }

    private void CheckForVictoryCollider()
    {
        RaycastHit2D[] raycastHit2D = Physics2D.BoxCastAll(victoryCollider.transform.position, victoryCollider.bounds.size * 1.8f, 0f, victoryCollider.transform.forward);

        foreach (RaycastHit2D hit in raycastHit2D)
        {
            Entities entity = hit.collider.gameObject.GetComponent<Player_Entity>();

            if (entity)
            {
                Victory();
                Debug.Log("Victory");
            }
        }
    }

    private void Victory()
    {
        Player_Menus_Manager.instance.TurnOnVictoryScene(victoryDialog);
        isVictory = true;

        Room_Intro[] rooms = FindObjectsOfType<Room_Intro>();

        foreach(Room_Intro room in rooms)
        {
            room.OpenExitDoor();    
        }
    }
}

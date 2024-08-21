using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Intro : Base_Room
{
    public Collider2D entryCollider;
    public Animator exitDoorAnimator;

    public Base_Room prevRoom;
    public Base_Room nextRoom;

    private bool playerCrossedToNextRoom;
    public override void Start()
    {
        base.Start();
    }


    void Update()
    {
        if (!playerCrossedToNextRoom)
        {
            DetectPlayerCrossingDoor();
        }
        
    }

    private void DetectPlayerCrossingDoor()
    {
        RaycastHit2D[] raycastHit2D = Physics2D.BoxCastAll(entryCollider.transform.position, entryCollider.bounds.size, 0f, entryCollider.transform.forward);

        foreach (RaycastHit2D hit in raycastHit2D)
        {
            Entities entity = hit.collider.gameObject.GetComponent<Player_Entity>();

            if (entity)
            {
                PlayerCrossToNextRoom();
            }
        }
    }

    private void PlayerCrossToNextRoom()
    {
        CloseExitDoor();
        playerCrossedToNextRoom = true;
    }

    public void OpenExitDoor()
    {
        exitDoorAnimator.SetTrigger("Open");
    }
    public void CloseExitDoor()
    {
        exitDoorAnimator.SetTrigger("Close");
    }
}

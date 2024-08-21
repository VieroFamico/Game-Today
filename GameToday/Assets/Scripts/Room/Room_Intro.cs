using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Intro : Base_Room
{
    public Collider2D getDialogCollider;
    public Collider2D crossTheDoorCollider;
    public Animator exitDoorAnimator;

    public Base_Room prevRoom;
    public Base_Room nextRoom;

    private bool playerGotDialog;
    private bool playerCrossedToNextRoom;
    public override void Start()
    {
        base.Start();
    }


    void Update()
    {
        if (!playerGotDialog)
        {
            DetectPlayerGettingDialog();
            return;
        }

        if (!playerCrossedToNextRoom)
        {
            DetectPlayerCrossingDoor();
        }
        
    }

    private void DetectPlayerGettingDialog()
    {
        RaycastHit2D[] raycastHit2D = Physics2D.BoxCastAll(getDialogCollider.transform.position, getDialogCollider.bounds.size, 0f, getDialogCollider.transform.forward);
        foreach (RaycastHit2D hit in raycastHit2D)
        {
            Entities entity = hit.collider.gameObject.GetComponent<Player_Entity>();
            Debug.Log(entity);
            if (entity)
            {
                PlayerGetIntroDialog();
            }
        }
    }

    private void DetectPlayerCrossingDoor()
    {
        RaycastHit2D[] raycastHit2D = Physics2D.BoxCastAll(crossTheDoorCollider.transform.position, crossTheDoorCollider.bounds.size, 0f, crossTheDoorCollider.transform.forward);

        foreach (RaycastHit2D hit in raycastHit2D)
        {
            Entities entity = hit.collider.gameObject.GetComponent<Player_Entity>();

            if (entity)
            {
                PlayerCrossToNextRoom();
            }
        }
    }

    private void PlayerGetIntroDialog()
    {
        playerGotDialog = true;
        OpenExitDoor();
    }

    private void PlayerCrossToNextRoom()
    {
        CloseExitDoor();
        playerCrossedToNextRoom = true;
        Invoke("Destroy(this)", 5f);
    }

    public void OpenExitDoor()
    {
        exitDoorAnimator.SetTrigger("OpenDoor");
    }
    public void CloseExitDoor()
    {
        exitDoorAnimator.SetTrigger("CloseDoor");
    }
}

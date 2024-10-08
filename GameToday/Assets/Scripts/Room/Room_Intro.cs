using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room_Intro : Base_Room
{
    [Header("References")]
    public Collider2D getDialogCollider;
    public Collider2D crossTheDoorCollider;
    public Animator exitDoorAnimator;
    public Light2D roomLight;

    public Dialog introDialog;

    public Base_Room prevRoom;
    public Base_Room nextRoom;

    private bool playerGotDialog;
    private bool playerCrossedToNextRoom;
    public bool dialogCompleted = false;
    public bool roomIsActive = false;
    public bool roomIsCompleted = false;

    void Start()
    {
        DialogManager.instance.dialogEnded.AddListener(OnDialogEnded);
    }

    void Update()
    {
        if (!playerGotDialog && !roomIsActive)
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

        DialogManager.instance.StartDialog(introDialog);
        dialogCompleted = false;
    }

    private void PlayerCrossToNextRoom()
    {

        CloseExitDoor();
        playerCrossedToNextRoom = true;
        roomLight.intensity = 0f;
        Destroy(this, 5f);
    }

    private void OnDialogEnded()
    {
        if (playerGotDialog)
        {
            dialogCompleted = true;
            ActivateRoom();
        }
    }

    private void OnDestroy()
    {
        if (DialogManager.instance != null)
        {
            DialogManager.instance.dialogEnded.RemoveListener(OnDialogEnded);
        }
    }

    public void ActivateRoom()
    {
        roomIsActive = true;

        Combat_Room_Module combatRoom = GetComponent<Combat_Room_Module>();
        Puzzle_Room_Module puzzleRoom = GetComponent<Puzzle_Room_Module>();
        if(!combatRoom && !puzzleRoom)
        {
            CompleteThisRoom();
        }
    }

    public void CompleteThisRoom()
    {
        PlayerState_Manager.instance.currentRoom = roomNumber;
        roomIsActive = false;
        roomIsCompleted = true;
        OpenExitDoor();
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

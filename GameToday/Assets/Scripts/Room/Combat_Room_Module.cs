using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Combat_Room_Module : MonoBehaviour
{
    public Room_Intro room;
    public AudioClip combatRoomSong;
    public bool isActive;

    [Header("Items")]
    public bool hasLoreItem;
    public Base_Item_ScriptableObject itemToDisplay;
    public Item_Container itemToDisplayOnPickUp;

    [Header("Pillars and Spawners")]
    public List<Pillar_Entity> roomPillars;
    public Enemy_Spawner[] spawners;

    private void Awake()
    {
        foreach (Pillar_Entity pillar_Entity in roomPillars)
        {
            pillar_Entity.room = room;
        }
    }
    void Start()
    {
        room = GetComponent<Room_Intro>();

        if (hasLoreItem)
        {
            itemToDisplayOnPickUp.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (hasLoreItem)
        {
            if (itemToDisplayOnPickUp.isPickedUp && !room.roomIsCompleted)
            {
                CompleteRoom();
            }
        }
        

        if (room.dialogCompleted && !isActive && !room.roomIsCompleted)
        {
            ActivateRoom();
        }
        else
        {
            if(!room.roomIsCompleted)
            {
                CheckPillarCharged();
            }
        }
    }

    private void ActivateRoom()
    {
        isActive = true;

        foreach(Pillar_Entity pillar in roomPillars)
        {
            pillar.StartRoom();
        }

        Audio_Manager.instance.PlaySong(combatRoomSong);
    }
    public List<Pillar_Entity> GetRoomPillars()
    {
        return roomPillars;
    }

    public virtual void CheckPillarCharged()
    {
        bool allIsCharged = true;
        foreach (Pillar_Entity pillar_Entity in roomPillars)
        {
            if (!pillar_Entity.isCharged)
            {
                allIsCharged = false;
                return;
            }
        }

        if (allIsCharged)
        {
            foreach (Enemy_Spawner spawner in spawners)
            {
                if (spawner)
                {
                    Destroy(spawner.gameObject);
                }
            }

            if (hasLoreItem && itemToDisplayOnPickUp)
            {
                DropFinalItem();
            }
            else
            {
                CompleteRoom();
            }
            
        }
    }
    public void DropFinalItem()
    {
        itemToDisplayOnPickUp.gameObject.SetActive(true);
    }
    public void CompleteRoom()
    {
        if (hasLoreItem)
        {
            Destroy(itemToDisplayOnPickUp.gameObject);
        }

        PlayerState_Manager.instance.FullRecovery();
        Audio_Manager.instance.PlayOpeningSong();

        isActive = false;
        ItemDisplay_Manager.instance.ShowItem(itemToDisplay);
        room.CompleteThisRoom();
    }
}

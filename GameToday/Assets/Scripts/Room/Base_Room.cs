using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Room : MonoBehaviour
{
    public List<Pillar_Entity> roomPillars;
    public int roomNumber;
    public enum RoomType
    {
        Combat,
        Puzzle,
    }

    public RoomType roomType;

    public virtual void Awake()
    {
        foreach(Pillar_Entity pillar_Entity in roomPillars)
        {
            pillar_Entity.room = this;
        }
    }

    public virtual void Start()
    { 
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            if(pillar_Entity.isCharged)
            {
                continue;
            }
            else
            {
                allIsCharged = false;
                return;
            }
        }

        if(allIsCharged)
        {
            CompleteThisRoom();
        }
    }

    public virtual void CompleteThisRoom()
    {
        StateManager_Player.instance.currentRoom = roomNumber;
    }
}

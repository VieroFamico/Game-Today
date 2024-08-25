using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Room_Module : MonoBehaviour
{
    public bool isActive;
    public Room_Intro room;

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
    }

    void Update()
    {
        if (room.dialogCompleted && !isActive && !room.roomIsCompleted)
        {
            isActive = true;
        }
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
            if (pillar_Entity.isCharged)
            {
                continue;
            }
            else
            {
                allIsCharged = false;
                return;
            }
        }

        if (allIsCharged)
        {
            CompleteRoom();
        }
    }

    public void CompleteRoom()
    {
        isActive = false;
        foreach (Enemy_Spawner spawner in spawners)
        {
            Destroy(spawner.gameObject, 2f);
        }
        room.CompleteThisRoom();
    }
}

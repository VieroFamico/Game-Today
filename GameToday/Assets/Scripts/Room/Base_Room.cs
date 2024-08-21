using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Room : MonoBehaviour
{
    public List<Pillar_Entity> roomPillars;
    public int roomNumber;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Room : MonoBehaviour
{
    public int roomNumber;
    public enum RoomType
    {
        Combat,
        Puzzle,
    }

    public RoomType roomType;

    
}

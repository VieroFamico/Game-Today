using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager_Player : MonoBehaviour
{
    public static StateManager_Player instance;
    public enum PlayerState
    {
        Moving,
        Dashing,
        Attacking,
    }

    public PlayerState state;

    public bool isMoving;
    public bool isAbleToMove = true;
    public bool isDashing;
    public bool isAbleToDash = true;
    public bool isAttacking;
    public bool isInHarmonyState;
    public bool isAbleToHarmonyState = false;

    public int currentRoom;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
    }
    void Update()
    {

    }

    public PlayerState GetState()
    {
        return state;
    }
}

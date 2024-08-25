using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Manager : MonoBehaviour
{
    public static PlayerState_Manager instance;

    public Player_Entity player;
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
    public bool isAbleToAttack = true;

    public bool isInHarmonyState;
    public bool isAbleToHarmonyState = false;

    public bool isHavingUIDisplayed = false;

    public bool isDead = false;
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

    public void InitializePlayer()
    {
        isAbleToMove = true;
        isAbleToDash = true;
        isAbleToAttack = true;
        isAbleToHarmonyState = true;
        isDead = false;
    }

    public void DisablePlayer()
    {
        isAbleToMove = false;
        isAbleToDash = false;
        isAbleToAttack = false;
        isAbleToHarmonyState = false;
    }

    public PlayerState GetState()
    {
        return state;
    }

    public void SetMoving_Dashing_Attacking(bool ableToMove, bool ableToDash, bool ableToAttack)
    {
        isAbleToMove = ableToMove;
        isAbleToDash = ableToDash;
        isAbleToAttack = ableToAttack;
    }

}
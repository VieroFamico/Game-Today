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
    public bool isDashing;
    public bool isAttacking;
    public bool isInHarmonyState;
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
    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerState GetState()
    {
        return state;
    }
}

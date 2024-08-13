using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager_Player : MonoBehaviour
{
    public enum PlayerState
    {
        Moving,
        Dashing,
        Attacking,
    }

    public PlayerState state;
    void Start()
    {
        
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

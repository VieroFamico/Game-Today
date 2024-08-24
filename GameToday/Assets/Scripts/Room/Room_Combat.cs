using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Combat : MonoBehaviour
{
    public bool isActive;

    public Enemy_Spawner[] spawners;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CompleteRoom()
    {
        isActive = false;
        foreach (Enemy_Spawner spawner in spawners)
        {
            Destroy(spawner.gameObject, 2f);
        }
    }
}

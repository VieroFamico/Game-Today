using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public Room_Combat room;

    public Transform spawnPoint;

    [Serializable]
    public class EnemyToSpawn
    {
        public Base_Enemy enemyType;
        public float enemySpawnTime; // How many seconds until spawn
    }

    public EnemyToSpawn enemiesToSpawn;


    private float currTime = 0f;

    void Update()
    {
        if(!room.isActive)
        {
            return;
        }

        CheckSpawn();
    }

    private void CheckSpawn()
    {
        if(currTime > enemiesToSpawn.enemySpawnTime)
        {
            Spawn(enemiesToSpawn.enemyType);
            currTime = 0;
        }
        currTime += Time.deltaTime;
    }

    private void Spawn(Base_Enemy enemy)
    {
        Base_Enemy spawnedEnemy = Instantiate(enemy, spawnPoint.position, Quaternion.identity, transform);

    }
}

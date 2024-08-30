using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    public Combat_Room_Module combatRoomModule;

    public Transform spawnPoint;

    [Serializable]
    public class EnemyToSpawn
    {
        public Base_Enemy enemyType;
        public float enemySpawnTime; // How many seconds until spawn
    }

    public EnemyToSpawn enemiesToSpawn;

    private Animator animator;

    private float currTime = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(!combatRoomModule.isActive)
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
        animator.SetTrigger("Spawning");
        Base_Enemy spawnedEnemy = Instantiate(enemy, spawnPoint.position, Quaternion.identity, transform);
        spawnedEnemy.room = combatRoomModule.room;
        spawnedEnemy.combatRoom = combatRoomModule;
    }
}

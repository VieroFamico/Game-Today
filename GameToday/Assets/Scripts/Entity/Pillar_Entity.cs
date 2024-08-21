using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar_Entity : Entities
{
    public Base_Room room;
    public float maxFillPercentage = 25f;
    public float fillRate = 1f;
    public float activateDistance;
    public float activateRate;
    public bool isActive;
    public bool isCharged { get; private set;}

    private Player_Entity player;

    private float currActivatePercentage; 
    public override void Start()
    {
        currentHP = maxHP;
        player = FindAnyObjectByType<Player_Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Charge();
        }
        else
        {
            GetPlayerActivatingInput();
        }
    }
    private void Charge()
    {
        currActivatePercentage += Time.deltaTime * fillRate;

        if(currActivatePercentage >= maxFillPercentage)
        {
            isCharged = true;
            room.CheckPillarCharged();
        }
    }
    public override void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;

            Deactivate();
            return;
        }
        else if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    private void Deactivate()
    {
        if(isActive) isActive = false;
    }


    private void GetPlayerActivatingInput()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < activateDistance)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Activating();
                
            }
        }
    }
    public void Activating()
    {
        currActivatePercentage += activateRate * Time.deltaTime;
    }
}

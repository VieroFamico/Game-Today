using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar_Entity : Entities
{

    public bool isActive;

    private float currActivatePercentage;
    public override void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void Activating()
    {

    }
}

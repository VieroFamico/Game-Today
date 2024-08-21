using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entities : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;

    public virtual void Start()
    {
        currentHP = maxHP;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
        {
            currentHP = 0;

            Dead();
            return;
        }
        else if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
    
    public virtual void Dead()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Entity : Entities
{
    private Rigidbody2D rb2d;
     

    public override void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    public override void Dead()
    {
        currentHP = 0;
        UpdateHealthSlider();
        PlayerState_Manager.instance.isDead = true;
        PlayerState_Manager.instance.DisablePlayer();
        Player_Menus_Manager.instance.ShowDeathMenu();
        base.Dead();
    }

    public void KnockBack(Vector2 knockBackDirection)
    {
        StartCoroutine(KnockBackStun());
        rb2d.AddForce(knockBackDirection * 4f, ForceMode2D.Impulse);
    }

    private IEnumerator KnockBackStun()
    {
        PlayerState_Manager.instance.isAbleToMove = false;

        yield return new WaitForSeconds(0.1f);

        PlayerState_Manager.instance.isAbleToMove = true;
        rb2d.velocity = Vector2.zero;
    }
}

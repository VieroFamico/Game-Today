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
        base.Dead();
    }

    public void KnockBack(Vector2 knockBackDirection)
    {
        StartCoroutine(KnockBackStun());
        rb2d.AddForce(knockBackDirection * 4f, ForceMode2D.Impulse);
    }

    private IEnumerator KnockBackStun()
    {
        StateManager_Player.instance.isAbleToMove = false;

        yield return new WaitForSeconds(0.1f);

        StateManager_Player.instance.isAbleToMove = true;
        rb2d.velocity = Vector2.zero;
    }
}

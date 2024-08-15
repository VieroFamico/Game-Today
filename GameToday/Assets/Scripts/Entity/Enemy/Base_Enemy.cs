using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Base_Enemy : Entities
{
    public Base_Room room;
    public float moveSpeed;
    public Collider2D damageHitBox;
    public int damage;
    public float attackRange;
    public float attackDuration;

    private Rigidbody2D rb2d;
    private Transform target;

    private bool isAttacking;

    public override void Start()
    {
        currentHP = maxHP;
    }

    
    void Update()
    {
        CheckForTarget();

        if(!isAttacking)
        {
            TargetInRange();
        }
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            MoveToTarget();
        }
        
    }

    private void CheckForTarget()
    {
        Player_Entity player = FindObjectOfType<Player_Entity>();

        float distance = Vector2.Distance(transform.position, player.transform.position);
        target = player.transform;

        List<Pillar_Entity> pillars = room.GetRoomPillars();

        foreach(Pillar_Entity pillar in pillars)
        {
            if (pillar.isActive == false) continue;

            float tempDistance = Vector2.Distance(pillar.transform.position, transform.position);
            if (tempDistance < distance)
            {
                distance = tempDistance;
                target = pillar.transform;
            }
        }
    }

    private void MoveToTarget()
    {
        if(target == null) return;

        Vector2 dir = target.position - transform.position;

        if(dir.x > 0)
        {
            transform.localScale = new Vector3(1 ,1 ,1); 
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        rb2d.MovePosition((Vector2)transform.position + moveSpeed * Time.deltaTime * dir.normalized);
    }

    private void TargetInRange()
    {
        if(Vector2.Distance(target.transform.position, transform.position) < attackRange / 2)
        {
            Vector2 dir = target.position - transform.position;

            damageHitBox.transform.position = (Vector2)transform.position + dir.normalized;
            damageHitBox.transform.LookAt((Vector2)transform.position + dir);
            DealDamage();
        }
            

    }
    private IEnumerator DealDamage()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.1f);

        RaycastHit2D[] raycastHit2D = Physics2D.BoxCastAll(damageHitBox.transform.position, damageHitBox.bounds.size, 0f, damageHitBox.transform.forward);

        foreach(RaycastHit2D hit in raycastHit2D)
        {
            Entities entity = hit.collider.gameObject.GetComponent<Player_Entity>();

            if (entity)
            {
                entity.TakeDamage(damage);
                yield return new WaitForSeconds(0.1f);

                isAttacking = false;
            }

            entity = hit.collider.gameObject.GetComponent<Pillar_Entity>();

            if (entity)
            {
                entity.TakeDamage(damage);
                yield return new WaitForSeconds(0.1f);

                isAttacking = false;
            }

        }
    }
}

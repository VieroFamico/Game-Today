using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Base_Enemy : Entities
{
    [Header("References")]
    public Base_Room room;
    public Combat_Room_Module combatRoom;
    public Collider2D damageHitBox;
    public Animator attackAnimator;
    private Animator enemyAnimator;

    [Header("Attributes")]
    public float moveSpeed;
    public int damage;
    public float attackRange;
    public float attackDuration;


    private Rigidbody2D rb2d;
    private Transform target;

    private bool isAbleToMove = true;
    private bool isAttacking = false;

    public override void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
    }

    
    void Update()
    {
        CheckForTarget();

        if(!isAttacking && isAbleToMove)
        {
            TargetInRange();
        }
    }

    private void FixedUpdate()
    {
        if (!isAttacking && isAbleToMove)
        {
            MoveToTarget();
        }
        
    }

    private void CheckForTarget()
    {
        Player_Entity player = FindObjectOfType<Player_Entity>();

        float distance = Vector2.Distance(transform.position, player.transform.position);
        target = player.transform;

        List<Pillar_Entity> pillars = combatRoom.GetRoomPillars();

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

        AnimateMovement(dir);
    }

    private void TargetInRange()
    {
        if(Vector2.Distance(target.transform.position, transform.position) < attackRange / 1.5f && !isAttacking)
        {
            Vector2 dir = target.position - transform.position;

            Attack(dir);
            rb2d.velocity = Vector2.zero;
        }
    }

    private void Attack(Vector2 attackDirection)
    {
        isAttacking = true;

        float f = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;

        damageHitBox.transform.rotation = Quaternion.Euler(0f, 0f, f);
        attackAnimator.transform.rotation = Quaternion.Euler(0f, 0f, f);

        AnimateAttack(attackDirection);
        
        StartCoroutine(DealDamage());
    }

    private IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(attackDuration * 0.5f);

        float timeLeft = attackDuration - attackDuration * 0.5f;

        RaycastHit2D[] raycastHit2D = Physics2D.BoxCastAll(damageHitBox.transform.position, damageHitBox.bounds.size * 1.8f, 0f, damageHitBox.transform.forward);

        foreach(RaycastHit2D hit in raycastHit2D)
        {
            Entities entity = hit.collider.gameObject.GetComponent<Player_Entity>();

            if (entity)
            {
                entity.TakeDamage(damage);

                Vector2 dir = (entity.transform.position - transform.position).normalized;

                entity.GetComponent<Player_Entity>().KnockBack(dir);
                Debug.Log("hit");
                yield return new WaitForSeconds(timeLeft);
                break;
            }

            entity = hit.collider.gameObject.GetComponent<Pillar_Entity>();

            if (entity)
            {
                entity.TakeDamage(damage);
                yield return new WaitForSeconds(timeLeft);
                break;
            }
        }
        isAttacking = false;
    }

    public override void Dead()
    {
        base.Dead();
        Destroy(gameObject);
    }

    private void AnimateAttack(Vector2 attackDirection)
    {
        enemyAnimator.SetTrigger("Attacking");
        attackAnimator.SetTrigger("Attacking");

        if (attackDirection.x >= 0f)
        {
            attackAnimator.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            attackAnimator.transform.localScale = new Vector3(-1, 1, 1);
        }

    }
    private void AnimateMovement(Vector2 movementDirection)
    {
        /*if(rb2d.velocity.magnitude > 0.1f)
        {
            enemyAnimator.SetBool("Moving", true);
        }
        else
        {
            enemyAnimator.SetBool("Moving", false);
        }*/
        
        if(movementDirection.x >= 0f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void KnockBack(Vector2 knockBackDirection)
    {
        StartCoroutine(KnockBackStun());
        rb2d.AddForce(knockBackDirection * 2f, ForceMode2D.Impulse);
    }

    private IEnumerator KnockBackStun()
    {
        isAbleToMove = false;

        yield return new WaitForSeconds(0.4f);

        isAbleToMove = true;
        rb2d.velocity = Vector2.zero;
    }
}

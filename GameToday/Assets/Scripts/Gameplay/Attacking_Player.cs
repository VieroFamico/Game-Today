using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking_Player : MonoBehaviour
{
    public Collider2D attack1Collider;
    public float BA_PushForce;
    public float BA_CancelDuration; //BA = basic attack
    public float BA_Duration;
    public int damage;

    private Animator playerAnimation;

    private Vector2 attackTargetPos;
    private int attackStage = 0;
    private float currBACancelDuration = 0;

    void Start()
    {
        playerAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetAttackInput();
    }

    private void GetAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && !StateManager_Player.instance.isAttacking)
        {
            attackTargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Attack();
        }

        if(currBACancelDuration >= BA_CancelDuration)
        {
            attackStage = 0;
        }

        currBACancelDuration += Time.deltaTime;
    }

    private void Attack()
    {
        if(attackStage == 0)
        {
            playerAnimation.SetTrigger("Attack1");
            attackStage++;

            currBACancelDuration = 0f;

            StartCoroutine(AttackingProcess());

        }
        else if(attackStage == 1)
        {
            playerAnimation.SetTrigger("Attack2");
            attackStage++;

            currBACancelDuration = 0f;

            StartCoroutine(AttackingProcess());

        }
        else if (attackStage == 3)
        {
            playerAnimation.SetTrigger("Attack3");
            attackStage++;

            currBACancelDuration = 0f;

            StartCoroutine(AttackingProcess());
        }
    }

    private IEnumerator AttackingProcess()
    {
        StateManager_Player.instance.isAttacking = true;

        yield return new WaitForSeconds(BA_Duration/2);

        RaycastHit2D[] raycastHit2D = Physics2D.BoxCastAll(attack1Collider.transform.position, attack1Collider.bounds.size, 0f, attack1Collider.transform.forward);

        foreach (RaycastHit2D hit in raycastHit2D)
        {
            Entities entity = hit.collider.gameObject.GetComponent<Player_Entity>();

            if (entity)
            {
                entity.TakeDamage(damage);
                
            }

            entity = hit.collider.gameObject.GetComponent<Pillar_Entity>();

            if (entity)
            {
                entity.TakeDamage(damage);

            }

        }

        yield return new WaitForSeconds(BA_Duration / 2);

        StateManager_Player.instance.isAttacking = false;
    }
}

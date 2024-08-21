using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking_Player : MonoBehaviour
{
    [Header("References")]
    public List<AudioClip> BA_AudioClips;
    public AudioSource playerAudioSource;
    public Collider2D attackCollider;
    public Transform swordTransform;

    private Animator playerAnimator;
    private Animator swordAnimator;

    [Header("Variables")]
    public float BA_PushForce;
    public float BA_CancelDuration; //BA = basic attack
    public float BA_Duration;
    public int damage;

    

    private Vector2 attackTargetPos;
    private int attackStage = 0;
    private float currBACancelDuration = 0;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        swordAnimator = swordTransform.GetComponent<Animator>();
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

        if (StateManager_Player.instance.isAttacking)
        {
            playerAnimator.SetBool("Attacking", true);
            swordAnimator.SetBool("Attacking", true);
        }
        else
        {
            playerAnimator.SetBool("Attacking", false);
            swordAnimator.SetBool("Attacking", false);
        }
    }

    private void Attack()
    {
        if(attackStage == 0)
        {
            AnimateBasicAttack(attackStage);

            PlayBASound(attackStage);

            attackStage++;

            currBACancelDuration = 0f;

            StartCoroutine(AttackingProcess());

        }
        else if(attackStage == 1)
        {
            AnimateBasicAttack(attackStage);

            PlayBASound(attackStage);

            attackStage++;

            currBACancelDuration = 0f;

            StartCoroutine(AttackingProcess());

        }
        else if (attackStage == 2)
        {
            AnimateBasicAttack(attackStage);

            PlayBASound(attackStage);

            attackStage++;

            currBACancelDuration = 0f;

            StartCoroutine(AttackingProcess());
        }
    }

    private IEnumerator AttackingProcess()
    {
        StateManager_Player.instance.isAttacking = true;

        yield return new WaitForSeconds(BA_Duration/2);

        RaycastHit2D[] raycastHit2D = Physics2D.BoxCastAll(attackCollider.transform.position, attackCollider.bounds.size * 1.5f, 0f, attackCollider.transform.forward);

        foreach (RaycastHit2D hit in raycastHit2D)
        {
            Entities entity = hit.collider.gameObject.GetComponent<Base_Enemy>();

            if (entity)
            {
                entity.TakeDamage(damage);
                Debug.Log(entity);
            }
        }

        yield return new WaitForSeconds(BA_Duration / 2);

        StateManager_Player.instance.isAttacking = false;
    }

    private void PlayBASound(int index)
    {
        if (BA_AudioClips.Count <= 0)
        {
            return;
        }
        if (BA_AudioClips[index])
        {
            if (playerAudioSource.isPlaying)
            {
                playerAudioSource.Stop();
            }

            playerAudioSource.clip = BA_AudioClips[index];
            playerAudioSource.Play();
        }
    }

    private void AnimateBasicAttack(int attackStage)
    {
        Vector2 attackVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = (attackVector - (Vector2)transform.position).normalized;

        float tempVectorDistance = (attackStage + 1) * 0.1f;

        Vector3 diff = attackVector - (Vector2)transform.position;
        float f = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        
        

        if (MathF.Abs(direction.x) >= MathF.Abs(direction.y))
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            if (direction.x > 0)
            {
                //Right
                attackCollider.transform.rotation = Quaternion.Euler(0, 0, f);
                swordTransform.rotation = Quaternion.Euler(0, 0, f);

            }
            else
            {
                //Left
                attackCollider.transform.rotation = Quaternion.Euler(0, 0, f);
                swordTransform.rotation = Quaternion.Euler(0, 0, f - 180f);

            }
            Debug.Log(rot.eulerAngles);
            swordTransform.position = (Vector2)transform.position + (direction * 0.4f);
            
        }
        else
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            if (direction.y > 0)
            {
                //Up
                attackCollider.transform.rotation = Quaternion.Euler(0, 0, f);
                swordTransform.rotation = Quaternion.Euler(0, 0, f - 90f);

            }
            else
            {
                //Down
                attackCollider.transform.rotation = Quaternion.Euler(0, 0, f);
                swordTransform.rotation = Quaternion.Euler(0, 0, f - 270f);

            }
            Debug.Log(rot.eulerAngles);
            swordTransform.position = (Vector2)transform.position + (direction * 0.6f);
        }

        playerAnimator.SetFloat("HorizontalAttackDirection", direction.x * tempVectorDistance);
        playerAnimator.SetFloat("VerticalAttackDirection", direction.y * tempVectorDistance);

        swordAnimator.SetFloat("HorizontalAttackDirection", direction.x * tempVectorDistance);
        swordAnimator.SetFloat("VerticalAttackDirection", direction.y * tempVectorDistance);


    }
}

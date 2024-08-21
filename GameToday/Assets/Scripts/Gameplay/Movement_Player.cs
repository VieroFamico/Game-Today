using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement_Player : MonoBehaviour
{
    [Header("References")]
    public TrailRenderer dashTrails;
    public Slider dashCooldownSlider;
    private Rigidbody2D rb2d;
    private Animator animator;

    [Header("Variables")]
    public float moveSpeed;
    public float dashSpeed;
    public int dashAmount;
    public float dashDuration;
    public float dashCooldown;

    private Vector2 moveVector2;
    private Vector2 lastMoveVector2;

    private float currDashCooldown = 0f;
    private int currDashAmount = 0;
    private bool isDashing;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dashTrails.emitting = false;
        currDashCooldown = dashCooldown;
        currDashAmount = dashAmount;
    }

    void Update()
    {
        if(StateManager_Player.instance.isAbleToMove)
        {
            GetMoveInput();
            
        }
        DashCooldown();
    }

    void FixedUpdate()
    {
        if (StateManager_Player.instance.isAbleToMove)
        {
            if (!isDashing)
            {
                Movement();
                GetDashInput();
            }
        }
    }
    #region Input and Movement
    private void GetMoveInput()
    {
        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        moveVector2 = new Vector2(xMovement, yMovement);

        if(moveVector2.magnitude < 0.1f)
        {
            AnimateMovement(lastMoveVector2 * 0.1f);
            return;
        }
        lastMoveVector2 = moveVector2;
        AnimateMovement(moveVector2);
    }

    private void Movement()
    {
        rb2d.MovePosition((Vector2)transform.position + moveVector2 * moveSpeed * Time.deltaTime);
    }
    #endregion

    #region Dash

    private void GetDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && currDashAmount > 0)
        {
            StartCoroutine(DashProcess());

            currDashAmount--;
            if (currDashCooldown >= dashCooldown && currDashAmount < dashAmount) currDashCooldown = 0f;
        }
    }
    private void DashCooldown()
    {
        currDashCooldown += Time.deltaTime;

        if (currDashCooldown > dashCooldown)
        {
            currDashCooldown = dashCooldown;
            dashCooldownSlider.gameObject.SetActive(false);

            if (currDashAmount < dashAmount)
            {
                currDashAmount = dashAmount;
            }
        }
        else
        {
            dashCooldownSlider.gameObject.SetActive(true);
        }

        dashCooldownSlider.value = currDashCooldown / dashCooldown;
    }

    private IEnumerator DashProcess()
    {
        float durationCount = 0f;
        isDashing = true;
        Vector2 dashVector = moveVector2;

        dashTrails.emitting = true;

        while (durationCount < dashDuration)
        {
            rb2d.velocity = dashVector * dashSpeed;
            durationCount += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        isDashing = false;

        yield return new WaitForSeconds(0.1f);

        dashTrails.emitting = false;
    }
    #endregion

    #region Animation

    private void AnimateMovement(Vector2 moveVector)
    {

        animator.SetFloat("Horizontal", moveVector.x);
        animator.SetFloat("Vertical", moveVector.y);
    }


    #endregion
}

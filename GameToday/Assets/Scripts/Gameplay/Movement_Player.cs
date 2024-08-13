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

    [Header("Variables")]
    public float moveSpeed;
    public float dashSpeed;
    public int dashAmount;
    public float dashDuration;
    public float dashCooldown;


    private Vector2 moveVector2;

    private float dashCooldownCount = 0f;
    private bool isDashing;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        dashTrails.emitting = false;
    }

    void Update()
    {
        GetMoveInput();

        DashCooldown();
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Movement();
            Dash();
        }
    }
    #region Input and Movement
    private void GetMoveInput()
    {
        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        moveVector2 = new Vector2(xMovement, yMovement);
        Debug.Log(moveVector2);
        
    }

    private void Movement()
    {
        rb2d.MovePosition((Vector2)transform.position + moveVector2 * moveSpeed * Time.deltaTime);
    }
    #endregion

    #region Dash

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownCount >= dashCooldown)
        {
            StartCoroutine(DashProcess());
        }
    }
    private void DashCooldown()
    {
        dashCooldownCount += Time.deltaTime;

        if (dashCooldownCount > dashCooldown)
        {
            dashCooldownCount = dashCooldown;
            dashCooldownSlider.gameObject.SetActive(false);
        }
        else
        {
            dashCooldownSlider.gameObject.SetActive(true);
        }

        dashCooldownSlider.value = dashCooldownCount / dashCooldown;
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

        dashCooldownCount = 0f;
        isDashing = false;

        yield return new WaitForSeconds(0.1f);

        dashTrails.emitting = false;
    }
    #endregion
}

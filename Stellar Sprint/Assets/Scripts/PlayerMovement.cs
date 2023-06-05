using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Переменные / Variables
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerLife playerLife;
    private WeaponAttack weaponAttack;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private AudioSource jumpingSound;
    [SerializeField] private AudioSource jetpackSound;
    [SerializeField] private AudioSource landingSound;

    //Ввод движения по горизонтали, -1 - лево, 1 - право, 0 - на месте
    public float moveInput = 0f;
    private bool facingRight = true;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jumpReleaseModificator = 2f;

    private bool isJumping;
    private bool doubleJumpAviable;
    private bool isDoubleJumping;
    public bool hasJetpack;

    // Доля времени для прыжка в последний момент, зависая над обрывом, прежде чем упасть
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    // Доля времени для прыжка перед прикосновением к поверхности
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    public enum MovementState { idle, running, jumping, falling, jetpack, death, shoot, shootAndRun }

    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;
    public bool KnockFromRight;

    public bool isOnPlatform;
    public Rigidbody2D platformRb;

    #endregion
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerLife = GetComponent<PlayerLife>();
        weaponAttack = GetComponent<WeaponAttack>();
    }
    private void Update()
    {
        if (rb.bodyType != RigidbodyType2D.Static && playerLife.isPlayerAlive)
        {          
            if (moveInput < 0f && facingRight)
            {
                Flip();
            }
            else if (moveInput > 0f && !facingRight)
            {
                Flip();
            }

            // Прыжок

            if (IsGrounded())
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (jumpBufferCounter > 0f)
            {
                if(coyoteTimeCounter > 0f && !isJumping)
                {
                    jumpingSound.Play();
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    jumpBufferCounter = 0f;
                    StartCoroutine(JumpCooldown());
                    if (hasJetpack)
                    {
                        doubleJumpAviable = true;
                    }
                }
                else if (doubleJumpAviable)
                {
                    isDoubleJumping = true;
                    jetpackSound.Play();
                    doubleJumpAviable = false;
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.50f);
                }
            }

            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / jumpReleaseModificator);
                coyoteTimeCounter = 0f;
            }

            // Максимальная скорость падения
            if (rb.velocity.y < -50f)
            {
                rb.velocity = new Vector2(rb.velocity.x, -50f);
            }
        }

        UpdateAnimation();
    }
    private void FixedUpdate()
    {
        // Горизонтальное передвижение
        moveInput = Input.GetAxisRaw("Horizontal");
        if (KBCounter <= 0)
        {
            if (isOnPlatform)
            {
                rb.velocity = new Vector2((moveInput * moveSpeed) + platformRb.velocity.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            }
        }
        // Отбрасывание от получения урона
        else
        {
            if (KnockFromRight == true)
            {
                rb.velocity = new Vector2(-KBForce, KBForce);
            }
            if (KnockFromRight == false)
            {
                rb.velocity = new Vector2(KBForce, KBForce);
            }

            KBCounter -= Time.deltaTime;
        }
    }

    //Смена анимации игрока
    private void UpdateAnimation()
    {
        MovementState state;


        if (moveInput > 0f || moveInput < 0f)
        {
            if (weaponAttack.isShooting && !(rb.velocity.y > 0.1f || rb.velocity.y < -0.1f))
            {
                weaponAttack.isShooting = false;
                state = MovementState.shootAndRun;
            }
            else
            {
                state = MovementState.running;
            }
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f && !isDoubleJumping)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f && !isDoubleJumping)
        {
            state = MovementState.falling;
        }

        if (isDoubleJumping)
        {
            state = MovementState.jetpack;
            isDoubleJumping = false;
        }

        if (playerLife.isPlayerAlive == false)
        {
            state = MovementState.death;
        }

        if (weaponAttack.isShooting)
        {
            weaponAttack.isShooting = false;
            rb.velocity = new Vector2(rb.velocity.x / 2, rb.velocity.y);
            state = MovementState.shoot;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, jumpableGround);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool canPlayJumpLandingSound;
        if (collision.gameObject.name == "Terrain")
        {
            canPlayJumpLandingSound = true;
            if (!landingSound.isPlaying && canPlayJumpLandingSound == true)
            {
                canPlayJumpLandingSound = false;
                landingSound.Play();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
}

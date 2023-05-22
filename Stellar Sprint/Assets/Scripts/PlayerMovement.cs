using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //��������� ��� ����������� �����������
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;

    //�������� ��� �����������, �� ������� ����� �������
    [SerializeField] private LayerMask jumpableGround;

    //���� �������� �� �����������, -1 - ����, 1 - �����, 0 - �� �����
    private float moveInput = 0f;
    private bool jumpInputReleased = false;
    private bool facingRight = true;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float jumpReleaseModificator = 2f;

    [SerializeField] private AudioSource jumpingSound;
    private enum MovementState { idle, running, jumping, falling }

    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;
    public bool KnockFromRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (rb.bodyType != RigidbodyType2D.Static)
        {
            //�������������� ��������
            moveInput = Input.GetAxisRaw("Horizontal");

            if (KBCounter <= 0)
            {
                rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            }
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

            if (moveInput < 0f && facingRight)
            {
                Flip();
            }
            else if (moveInput > 0f && !facingRight)
            {
                Flip();
            }

            //������
            jumpInputReleased = Input.GetButtonUp("Jump");
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                jumpingSound.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            //������ ������ � ����������� �� ������� ������ ������
            //���� ������ ������ �������� � ������� �������� ������� ������ ���� �� �������� ������� ����� �������� ��
            //�������� ���������� jumpReleaseModificator, � �������� ����� ������������ ������ �� �������� ���������� �������� �������
            if (jumpInputReleased && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / jumpReleaseModificator);
            }

            // ������������ �������� �������
            if (rb.velocity.y < -50f)
            {
                rb.velocity = new Vector2(rb.velocity.x, -50f);
            }
        }

        UpdateAnimation();
    }

    //����� �������� ������
    private void UpdateAnimation()
    {

        MovementState state;
                
        if (moveInput > 0f)
        {
            state = MovementState.running;
        }
        else if (moveInput < 0f)
        {
            state = MovementState.running;
        }
        else 
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0); 
    }
}

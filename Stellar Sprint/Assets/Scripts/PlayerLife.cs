using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    // Layer names
    int PlayerLayer;
    int EnemyLayer;
    int ProjectileLayer;
    int TrapLayer;

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private Text hpText;

    [Header("Health")]
    public float maxHealth;
    public float health;
    public bool isPlayerAlive = true;
    private bool isCollidingWithTrap = false;

    public bool isHurt;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite damagingHeart;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    public Vector2 startPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();

        health = maxHealth;
        startPosition = transform.position;
    }
    private void Awake()
    {
        PlayerLayer = LayerMask.NameToLayer("Player");
        EnemyLayer = LayerMask.NameToLayer("Enemy");
        ProjectileLayer = LayerMask.NameToLayer("Projectile");
        TrapLayer = LayerMask.NameToLayer("Trap");

        if (isPlayerAlive)
        {
            Physics2D.IgnoreLayerCollision(PlayerLayer, EnemyLayer, false);
            Physics2D.IgnoreLayerCollision(PlayerLayer, ProjectileLayer, false);
            Physics2D.IgnoreLayerCollision(PlayerLayer, TrapLayer, false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                if (isHurt)
                {
                    StartCoroutine(HeartDamageEffect(i));
                }
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
    private void FixedUpdate()
    {
        hpText.text = "Состояние шлема: " + health;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            health = 0;
            isPlayerAlive = false;
        }

        if (isHurt)
        {
            anim.SetTrigger("hurt");
            isHurt = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            if (!isCollidingWithTrap)
            {
                isCollidingWithTrap = true;

                PlayerTakeDamage(1);

                if (isPlayerAlive)
                {
                    transform.position = startPosition;
                }
                else
                {
                    Die();
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            isCollidingWithTrap = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            startPosition = collision.transform.position;
            if (collision.TryGetComponent<Animator>(out Animator checkpointAnimator))
            {
                checkpointAnimator.enabled = true;
            }
        }
    }

    public void PlayerTakeDamage(float damage)
    {
        health -= damage;
        hurtSound.Play();

        if (health > 0f)
        {
            isHurt = true;
            StartCoroutine(Invulnerability());
        }
        if (health == 0f)
        {
            Die();
        }
    }

    private IEnumerator Invulnerability()
    {
        if (isPlayerAlive)
        {
            Physics2D.IgnoreLayerCollision(PlayerLayer, EnemyLayer, true);
            Physics2D.IgnoreLayerCollision(PlayerLayer, ProjectileLayer, true);
            Physics2D.IgnoreLayerCollision(PlayerLayer, TrapLayer, true);
            for (int i = 0; i < numberOfFlashes; i++)
            {
                spriteRend.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
                spriteRend.color = Color.white;
                yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            }
            Physics2D.IgnoreLayerCollision(PlayerLayer, EnemyLayer, false);
            Physics2D.IgnoreLayerCollision(PlayerLayer, ProjectileLayer, false);
            Physics2D.IgnoreLayerCollision(PlayerLayer, TrapLayer, false);
        }
    }
    private IEnumerator HeartDamageEffect(int i)
    {
        for (int j = 0; j < numberOfFlashes; j++)
        {
            hearts[i].sprite = damagingHeart;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            hearts[i].sprite = emptyHeart;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
    }

    private void Die()
    {
        hurtSound.Stop();
        deathSound.Play();
        isPlayerAlive = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
        Physics2D.IgnoreLayerCollision(PlayerLayer, EnemyLayer, true);
        Physics2D.IgnoreLayerCollision(PlayerLayer, ProjectileLayer, true);
        Physics2D.IgnoreLayerCollision(PlayerLayer, TrapLayer, true);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

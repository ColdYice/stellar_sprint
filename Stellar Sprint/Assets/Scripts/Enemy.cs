using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 10f;
    [SerializeField] GameObject deathEffect;
    private Animator anim;

    public int damage;
    private bool isEnemyAlive = true;
    private PlayerLife playerLife;
    private PlayerMovement playerMovement;

    [Header("Damaged Effect")]
    [SerializeField] private float damagedDffectDuration = 0.1f;
    [SerializeField] private int numberOfFlashes = 1;
    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private AudioSource deathSound;

    private SpriteRenderer spriteRend;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;
    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        playerLife = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }
    private void FixedUpdate()
    {
        if (health <= 0f)
        {
            health = 0f;
            Die();
        }
    }

    // Knockback
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isEnemyAlive)
        {
            if (collision.gameObject.tag == "Player")
            {
                KnockPlayerBack(collision);
                playerLife.PlayerTakeDamage(damage);
            }
            if (collision.gameObject.tag == "Trap")
            {
                Die();
            }
        }
    }

    // Damage from player and enemy death
    public void EnemyTakeDamage(float damage )
    {
        if (isEnemyAlive)
        {
            health -= damage;
            hurtSound.Play();

            if (health > 0f)
            {
                anim.SetTrigger("hurt");
                StartCoroutine(Damaged());
            }
            if (health <= 0f)
            {
                Die();
            }
        }
        
    }
    private IEnumerator Damaged()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.material.shader = shaderGUItext;
            spriteRend.color = Color.white; 
            yield return new WaitForSeconds(damagedDffectDuration);

            spriteRend.color = Color.white; 
            spriteRend.material.shader = shaderSpritesDefault;
            yield return new WaitForSeconds(damagedDffectDuration);
        }
    }

    public void KnockPlayerBack(Collision2D collision)
    {
        playerMovement.KBCounter = playerMovement.KBTotalTime;

        if (collision.transform.position.x <= transform.position.x)
        {
            playerMovement.KnockFromRight = true;
        }
        if (collision.transform.position.x >= transform.position.x)
        {
            playerMovement.KnockFromRight = false;
        }
    }

    void Die()
    {
        if(gameObject.name == "Flying")
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.freezeRotation = false;
        }
        anim.SetBool("death", true);
        isEnemyAlive = false;
        spriteRend.color =  new Color(0.3921569f, 0.3921569f, 0.3921569f, 1); // Grey color - R - 100, G - 100, B - 100, Opacity - 255
        if (anim.parameters.Any(x => x.name == "hurt")) anim.ResetTrigger("hurt");
        if (anim.parameters.Any(x => x.name == "attack")) anim.ResetTrigger("attack");        
        if (TryGetComponent(out EnemyPatrol enemyPatrol)) GetComponent<EnemyPatrol>().enabled = false;
        if (TryGetComponent(out SelfDestruction selfDestruction)) GetComponent<SelfDestruction>().enabled = true;
        if (TryGetComponent(out WayponitFollower wayponitFollower)) GetComponent<WayponitFollower>().enabled = false;
        gameObject.layer = 6; // 6 - Corpse layer
        deathSound.Play();
        this.enabled = false;
    }
}
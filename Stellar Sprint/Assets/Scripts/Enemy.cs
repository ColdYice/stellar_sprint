using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float damage;
    public PlayerLife playerLife;
    public PlayerMovement playerMovement;

    [SerializeField] private float health = 100f;
    [SerializeField] GameObject deathEffect;

    // Knockback
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
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
            playerLife.PlayerTakeDamage(damage);
        }
    }

    // Damage from player and enemy death
    public void EnemyTakeDamage(float damage )
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
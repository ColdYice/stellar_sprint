using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{

    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletDamage = 1f;
    [SerializeField] private GameObject impactEffect;
    private Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, 2f);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        //Определяет, попала ли пуля по ИГРОКУ
        PlayerLife player = hitInfo.GetComponent<PlayerLife>();
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (player != null)
        {
            player.PlayerTakeDamage(bulletDamage);
        }
        if (enemy != null)
        {
            enemy.EnemyTakeDamage(bulletDamage * 2);
        }
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);

    }
}

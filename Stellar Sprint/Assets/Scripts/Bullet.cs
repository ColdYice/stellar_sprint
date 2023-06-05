using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletDamage = 2f;
    [SerializeField] private GameObject impactEffect;
    private Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, 0.7f);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed; 
        Physics2D.IgnoreLayerCollision(7, 12, true);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.layer != 11)
        {
            //Определяет, попала ли пуля по врагу
            Enemy enemy = hitInfo.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(bulletDamage);
            }

            Instantiate(impactEffect, transform.position, transform.rotation);

            Destroy(gameObject); 
        }
    }
}

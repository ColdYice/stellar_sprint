using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletDamage = 20f;
    [SerializeField] private GameObject impactEffect;
    private Rigidbody2D rb;

    void Start()
    {
        StartCoroutine(SelfDestruct());
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed;        
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Banana") == false)
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

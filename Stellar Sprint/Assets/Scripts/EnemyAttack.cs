using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject enemyProjectile;
    [SerializeField] private AudioSource shootSound;

    //[SerializeField] private float fireCooldown = 1f;
    //private float currentCooldown;

    public void ShootPlayer()
    {
        shootSound.Play();
        Instantiate(enemyProjectile, firePoint.position, firePoint.rotation);
        //if (currentCooldown <= 0)
        //{
        //    currentCooldown = fireCooldown;
        //}
        //else { currentCooldown -= Time.deltaTime; }
    }
}

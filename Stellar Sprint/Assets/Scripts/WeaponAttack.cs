using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioSource shootSound;
    private PlayerLife playerLife;

    [SerializeField] private float fireCooldown = 0.7f;
    private float currentCooldown;
    public bool isShooting = false;
    private void Start()
    {
        playerLife = GetComponent<PlayerLife>();
    }
    void Update()
    {
        if (!isShooting && Input.GetButton("Fire1") && currentCooldown <= 0 && playerLife.isPlayerAlive)
        {
            currentCooldown = fireCooldown;
            isShooting = true;
        }
        else currentCooldown -= Time.deltaTime;
    }

    private void Shoot()
    {
        shootSound.Play();
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}

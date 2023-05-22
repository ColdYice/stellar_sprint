using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bullet;

    [SerializeField] private float fireCooldown = 0.7f;
    private float currentCooldown;

    void Update()
    {
        if (currentCooldown <= 0 && Input.GetButton("Fire1"))
        {
            currentCooldown = fireCooldown;
            Shoot();
        }
        else currentCooldown -= Time.deltaTime;
    }

    private void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float fireRate = 0.2f;
    public float bulletSpeed = 300f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    float attackTimer;
    Transform target;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            target = other.transform;
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (attackTimer >= fireRate)
            {
                Shoot();
                attackTimer = 0;
            }
        }
        attackTimer += Time.deltaTime;
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
        Destroy(bullet, 2f);
    }
}

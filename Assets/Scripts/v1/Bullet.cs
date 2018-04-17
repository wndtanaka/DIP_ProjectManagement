using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public GameObject bulletPartEnvironment;
    public GameObject bulletPartEnemy;

    Transform contactPoint;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Not Destroyable"))
        {
            Destroy(gameObject);
            contactPoint = other.transform;
            SpawnParticleSystem();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
            contactPoint = other.transform;
            DealDamage(contactPoint);
            SpawnParticleSystemEnemy();
        }
        else
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            contactPoint = other.transform;
            SpawnParticleSystem();
        }
    }
    void SpawnParticleSystem()
    {
        GameObject part = Instantiate(bulletPartEnvironment, contactPoint.position, contactPoint.rotation);
        Destroy(part, 1f);
    }
    void SpawnParticleSystemEnemy()
    {
        GameObject part = Instantiate(bulletPartEnemy, contactPoint.position, contactPoint.rotation);
        Destroy(part, 1f);
    }
    void DealDamage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        e.TakeDamage(damage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmModifier : MonoBehaviour
{
    public float damage = 25f;
    public float armModifier = 0.5f;

    public float attackSpeed;

    Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void CountDamage(float amount)
    {
        amount *= armModifier;
        enemy.TakeDamage(amount);
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("OIII");
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}

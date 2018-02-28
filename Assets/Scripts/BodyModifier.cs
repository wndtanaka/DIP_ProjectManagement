using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyModifier : MonoBehaviour
{
    public float bodyModifier = 1f;

    Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void CountDamage(float amount)
    {
        amount *= bodyModifier;
        enemy.TakeDamage(amount);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegModifier : MonoBehaviour
{
    public float legModifier = 0.5f;

    Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void CountDamage(float amount)
    {
        amount *= legModifier;
        enemy.TakeDamage(amount);
    }
}

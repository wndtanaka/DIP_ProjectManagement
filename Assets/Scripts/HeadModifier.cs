using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadModifier : MonoBehaviour
{
    public float headModifier = 2f;

    Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void CountDamage(float amount)
    {
        amount *= headModifier;
        enemy.TakeDamage(amount);
    }
}

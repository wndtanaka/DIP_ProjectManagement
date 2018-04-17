using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmModifier : MonoBehaviour
{
    public float damage = 25f;
    public float armModifier = 0.5f;

    public float attackSpeed;

    Enemy enemy;
    GameObject player;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        
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
            player.KnockBack(0.5f, Vector3.back);
            player.TakeDamage(10);
        }
    }
}

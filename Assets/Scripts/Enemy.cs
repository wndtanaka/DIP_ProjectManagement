using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float curHealth = 100;
    public float damage;
    public float movementSpeed;
    public float maxHealth = 100;

    public Image healthBar;

    // Use this for initialization
    void Start()
    {
        maxHealth = curHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float amount)
    {
        curHealth -= amount;
        healthBar.fillAmount = curHealth / maxHealth;
        if (curHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}

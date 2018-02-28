using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float curHealth = 100;
    public float movementSpeed;
    public float maxHealth = 100;

    public Image healthBar;
    public GameObject healthBG;

    // Use this for initialization
    void Start()
    {
        maxHealth = curHealth;
    }

    public void TakeDamage(float amount)
    {
        curHealth -= amount;
        healthBar.fillAmount = curHealth / maxHealth;
        if (curHealth <= 0)
        {
            Die();
        }
        ShowHealthBar();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void ShowHealthBar()
    {
        healthBG.SetActive(true);
    }
}

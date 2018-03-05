using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;

    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;

    public Text healthText;
    public Transform spawnPoint;
    public bool spawnPlayer = false;

    bool isSpawned = false;
    RigidbodyFirstPersonController rigid;
    Rigidbody rb;

    private void Start()
    {
        health = maxHealth;
        rigid = GetComponent<RigidbodyFirstPersonController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isSpawned != spawnPlayer)
        {
            Respawn();
            spawnPlayer = false;
        }
        else
        {
            isSpawned = spawnPlayer;
        }
        healthText.text = health + "/" + maxHealth;

        if (knockBackCounter > 0)
        {
            rigid.enabled = false;
            knockBackCounter -= Time.deltaTime;
        }
        else if (knockBackCounter <= 0)
        {
            rigid.enabled = true;
        }

        if (health == 0)
        {
            Die();
        }
    }

    void Respawn()
    {
        transform.position = spawnPoint.position;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }

    public void KnockBack(float knock, Vector3 direction)
    {
        knockBackCounter = knock;
        //rb.AddForce(direction * 100 * Time.deltaTime, ForceMode.Impulse);
    }

    void Die()
    {
        Debug.Log("You Died");
    }
}

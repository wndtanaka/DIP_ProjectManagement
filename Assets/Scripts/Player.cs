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

    private void Start()
    {
        health = maxHealth;
        rigid = GetComponent<RigidbodyFirstPersonController>();
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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            KnockBack(2);
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

    public void KnockBack(Vector3 knock)
    {
        knockBackCounter = knock;
    }
}

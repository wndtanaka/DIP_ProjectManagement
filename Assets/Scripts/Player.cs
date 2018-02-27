using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform spawnPoint;
    public bool spawnPlayer = false;

    bool isSpawned = false;

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
    }

    void Respawn()
    {
        transform.position = spawnPoint.position;
    }
}

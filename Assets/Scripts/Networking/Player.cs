using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    [RequireComponent(typeof(PlayerSetup))]
    public class Player : NetworkBehaviour
    {
        [SerializeField]
        private int maxHealth = 100;

        [SerializeField]
        private Behaviour[] disableOnDeath;
        private bool[] wasEnabled;

        [SerializeField]
        private GameObject[] disableGameObjectOnDeath;

        [SerializeField]
        private GameObject deathEffect;

        [SerializeField]
        private GameObject spawnEffect;

        [SyncVar]
        private int currentHealth;

        [SyncVar]
        private bool _isDead = false;
        public bool isDead
        {
            get
            {
                return _isDead;
            }
            protected set
            {
                _isDead = value;
            }
        }

        PlayerController controller;
        private bool initialSetup = true;

        private void Start()
        {
            controller = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                RpcTakeDamage(999999);
            }
        }

        public void SetupPlayer()
        {
            if (isLocalPlayer)
            {
                // switch camera
                GameManager.instance.SetSceneCameraActive(false);
                GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
            }
            CmdBroadcastNewPlayerSetup();
        }

        [Command]
        private void CmdBroadcastNewPlayerSetup()
        {
            RpcSetupPlayerOnAllClients();
        }

        [ClientRpc]
        private void RpcSetupPlayerOnAllClients()
        {
            if (initialSetup)
            {
                wasEnabled = new bool[disableOnDeath.Length];
                for (int i = 0; i < wasEnabled.Length; i++)
                {
                    wasEnabled[i] = disableOnDeath[i].enabled;
                }
            }

            initialSetup = false;

            SetDefaults();
        }

        public void SetDefaults()
        {
            isDead = false;
            currentHealth = maxHealth;
            controller.thrusterFuelAmount = controller.maxThrusterFuel;

            // enable components
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = wasEnabled[i];
            }

            // enable gameObjects
            for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
            {
                disableGameObjectOnDeath[i].SetActive(true);
            }

            // enable colliders
            Collider _col = GetComponent<Collider>();
            if (_col != null)
            {
                _col.enabled = true;
            }

            // create spawneffect
            GameObject _spawnEffect = Instantiate(spawnEffect, transform.position, Quaternion.identity);
            Destroy(_spawnEffect, 3f);
        }

        [ClientRpc]
        public void RpcTakeDamage(int _amount)
        {
            if (isDead)
            {
                return;
            }
            currentHealth -= _amount;

            Debug.Log(transform.name + " now has " + currentHealth + " health");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;

            // disable components
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = false;
            }
            // disable gameObjects
            for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
            {
                disableGameObjectOnDeath[i].SetActive(false);
            }
            // disable collider
            Collider _col = GetComponent<Collider>();
            if (_col != null)
            {
                _col.enabled = false;
            }

            // create death effect
            GameObject _deathEffect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(_deathEffect, 3f);

            // switch camera
            if (isLocalPlayer)
            {
                GameManager.instance.SetSceneCameraActive(true);
                GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
            }

            Debug.Log(transform.name + " is DEAD!");

            StartCoroutine(Respawn());
        }

        IEnumerator Respawn()
        {
            yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
            Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
            transform.position = _spawnPoint.position;
            transform.rotation = _spawnPoint.rotation;
            // switch camera
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);

            yield return new WaitForSeconds(0.1f);

            SetupPlayer();

            Debug.Log(transform.name + " respawned.");
        }
    }
}
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    [RequireComponent(typeof(WeaponManager))]
    public class PlayerShoot : NetworkBehaviour
    {
        [SerializeField]
        private Camera cam;
        [SerializeField]
        private LayerMask layerMask;

        private const string PLAYER_TAG = "Player";

        private PlayerWeapon currentWeapon;
        private WeaponManager weaponManager;

        private void Start()
        {
            if (cam == null)
            {
                this.enabled = false;
            }
            weaponManager = GetComponent<WeaponManager>();
        }

        private void Update()
        {
            currentWeapon = weaponManager.GetCurrentWeapon();

            if (currentWeapon.fireRate <= 0f)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    CancelInvoke("Shoot");
                }
            }
        }

        // called on the server when a player shoots
        [Command]
        void CmdOnShoot()
        {
            RpcDoShootEffect();
        }

        // called on all clients when a player shoots
        [ClientRpc]
        void RpcDoShootEffect()
        {
            weaponManager.GetCurrentWeaponGraphics().muzzleFlash.Play();
        }

        // called on the server when we hit something
        // takes in the hit point and the normal of the surface
        [Command]
        void CmdOnHit(Vector3 _pos, Vector3 _normal)
        {
            RpcDoHitEffect(_pos, _normal);
        }

        // called on all clients
        // spawn hit effect
        [ClientRpc]
        void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
        {
            GameObject _hitEffect = Instantiate(weaponManager.GetCurrentWeaponGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
            Destroy(_hitEffect, 1f);
        }

        [Client]
        private void Shoot()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            // shooting, call the OnShoot method on the server
            CmdOnShoot();

            RaycastHit _hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, layerMask))
            {
                if (_hit.collider.tag == PLAYER_TAG)
                {
                    CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
                }
                // call the OnHit method on the server when we hit something
                CmdOnHit(_hit.point, _hit.normal);
            }
        }

        [Command]
        void CmdPlayerShot(string _playerID, int _damage)
        {
            Player _player = GameManager.GetPlayer(_playerID);
            _player.RpcTakeDamage(_damage);
        }
    }
}
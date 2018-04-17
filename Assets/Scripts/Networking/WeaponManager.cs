using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    public class WeaponManager : NetworkBehaviour
    {
        [SerializeField]
        private PlayerWeapon primaryWeapon;
        [SerializeField]
        private Transform weaponHolder;

        private PlayerWeapon currentWeapon;
        private WeaponGraphics currentGraphics;

        private const string WEAPON_LAYERNAME = "Weapon";

        private void Start()
        {
            EquipWeapon(primaryWeapon);
        }

        public PlayerWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }
        public WeaponGraphics GetCurrentWeaponGraphics()
        {
            return currentGraphics;
        }

        void EquipWeapon(PlayerWeapon _weapon)
        {
            currentWeapon = _weapon;
            GameObject _weaponInstance = Instantiate(currentWeapon.graphics,weaponHolder.position,weaponHolder.rotation);
            _weaponInstance.transform.SetParent(weaponHolder);

            currentGraphics = _weaponInstance.GetComponent<WeaponGraphics>();
            if (currentGraphics == null)
            {
                Debug.LogError("No WeaponGraphics component on the weapon object: " + _weaponInstance.name);
            }

            if (isLocalPlayer)
            {
                Utility.SetLayerRecursively(_weaponInstance,LayerMask.NameToLayer(WEAPON_LAYERNAME));
            }
        }
    }
}
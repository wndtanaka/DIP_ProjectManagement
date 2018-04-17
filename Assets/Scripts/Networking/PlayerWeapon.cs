using UnityEngine;

namespace Networking
{
    [System.Serializable]
    public class PlayerWeapon
    {
        public string name = "AR";
        public int damage = 10;
        public float range = 100f;

        public float fireRate = 0;

        public GameObject graphics;
    }
}
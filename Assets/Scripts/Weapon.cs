using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float shootRate = 15f;
    public int ammoCount = 30;
    public int maxAmmo = 90;
    public int maxMags = 30;

    public ParticleSystem muzzleFlash;
    public GameObject projectile;
    public Text ammoCounter;
    //public Recoil recoil;

    float fireRate = 0;
    Camera fpsCam;

    void Start()
    {
        fpsCam = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && ammoCount > 0 && Time.time >= fireRate)
        {
            fireRate = Time.time + 1f / shootRate;
            Shoot();
            ammoCount--;
            //recoil.StartRecoil(0.2f, 3f, 3f);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        ammoCounter.text = ammoCount + "/" + maxAmmo;
    }
    void Shoot()
    {
        muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            //Enemy enemy =  hit.transform.GetComponent<Enemy>();
            //if (enemy != null)
            //{
            //    enemy.TakeDamage(damage);
            //}

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Head"))
            {
                HeadModifier head = hit.transform.GetComponent<HeadModifier>();
                if (head != null)
                {
                    head.CountDamage(damage);
                }
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Body"))
            {
                BodyModifier body = hit.transform.GetComponent<BodyModifier>();
                if (body != null)
                {
                    body.CountDamage(damage);
                }
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Arm"))
            {
                ArmModifier arm = hit.transform.GetComponent<ArmModifier>();
                if (arm != null)
                {
                    arm.CountDamage(damage);
                }
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Leg"))
            {
                LegModifier leg = hit.transform.GetComponent<LegModifier>();
                if (leg != null)
                {
                    leg.CountDamage(damage);
                }
            }

            GameObject projectileGO = Instantiate(projectile, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(projectileGO, 1f);
        }
    }
    void Reload()
    {
        if (ammoCount >= maxMags)
        {
            return;
        }
        if (ammoCount < maxMags && maxAmmo > 0)
        {
            int neededAmmo;
            neededAmmo = maxMags - ammoCount;

            if (neededAmmo > maxAmmo)
            {
                ammoCount += maxAmmo;
                maxAmmo -= maxAmmo;
            }
            else
            {
                ammoCount = maxMags;
                maxAmmo -= neededAmmo;
            }

        }
    }
}

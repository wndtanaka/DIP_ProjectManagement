using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public float damage;
    public float range;
    public float shootRate;
    public int ammoCount;
    public int maxAmmo;
    public int maxMags;
    public float zoom;
    public bool ableToShoot = false;

    public ParticleSystem muzzleFlash;
    public GameObject projectile;
    public Text ammoCounter;
    public Text reloadText;
    //public Recoil recoil;
    int neededAmmo;
    protected float defaultFOV;
    protected float fireRate = 0;
    Camera fpsCam;
    protected GameObject crosshair;

    protected virtual void Start()
    {
        fpsCam = Camera.main;
        defaultFOV = fpsCam.fieldOfView;
        crosshair = GameObject.Find("Crosshair");
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButton(0) && ammoCount > 0 && Time.time >= fireRate && !ableToShoot)
        {
            fireRate = Time.time + 1f / shootRate;
            Shoot();
            ammoCount--;
            //recoil.StartRecoil(0.2f, 3f, 3f);
        }
        if (Input.GetMouseButton(0) && ammoCount <= 0)
        {
            StartCoroutine(ShowReloadText());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (ammoCount >= maxMags || maxAmmo <= 0)
            {
                return;
            }
            else
            {
                StartCoroutine(ReloadTime());
            }

        }
        ammoCounter.text = ammoCount + "/" + maxAmmo;

        if (Input.GetMouseButtonDown(1))
        {
            WeaponSwitching.isScoping = !WeaponSwitching.isScoping;
            Camera.main.fieldOfView = defaultFOV / zoom;
        }
        if (Input.GetMouseButtonUp(1))
        {
            WeaponSwitching.isScoping = !WeaponSwitching.isScoping;
            Camera.main.fieldOfView = defaultFOV;
        }
    }
    protected void Shoot()
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
    protected void Reload()
    {
        if (ammoCount < maxMags && maxAmmo > 0)
        {
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
    protected IEnumerator ShowReloadText()
    {
        reloadText.text = "Press 'R' to Reload";
        reloadText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        reloadText.gameObject.SetActive(false);
    }

    protected virtual IEnumerator ReloadTime()
    {
        ableToShoot = !ableToShoot;
        yield return new WaitForSeconds(2f);
        Reload();
        ableToShoot = !ableToShoot;
    }
}

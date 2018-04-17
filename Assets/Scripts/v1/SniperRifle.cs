using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : Weapon
{
    public float reloadTime;

    public Animator anim;
    public GameObject scopeIn;
    public Camera weaponCamera;

    bool isScoped = false;

    protected override void Update()
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
            isScoped = !isScoped;
            anim.SetBool("isScoped", isScoped); // return true
            if (isScoped)
            {
                StartCoroutine(OnScoped());
            }
            else
            {
                OnUnscoped();
            }
        }
    }

    IEnumerator OnScoped()
    {
        WeaponSwitching.isScoping = !WeaponSwitching.isScoping;
        yield return new WaitForSeconds(.15f);
        scopeIn.SetActive(isScoped);
        weaponCamera.gameObject.SetActive(!isScoped);
        crosshair.SetActive(!isScoped);
        Camera.main.fieldOfView = defaultFOV / zoom;
    }

    void OnUnscoped()
    {
        WeaponSwitching.isScoping = !WeaponSwitching.isScoping;
        scopeIn.SetActive(isScoped);
        weaponCamera.gameObject.SetActive(!isScoped);
        crosshair.SetActive(!isScoped);
        Camera.main.fieldOfView = defaultFOV;
    }

    protected override IEnumerator ReloadTime()
    {
        ableToShoot = !ableToShoot;
        anim.SetBool("isReloading", true);
        WeaponSwitching.isReloading = !WeaponSwitching.isReloading;
        yield return new WaitForSeconds(reloadTime);
        Reload();
        ableToShoot = !ableToShoot;
        anim.SetBool("isReloading", false);
        WeaponSwitching.isReloading = !WeaponSwitching.isReloading;
    }
}

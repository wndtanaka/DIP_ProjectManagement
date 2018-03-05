using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : Weapon
{
    public Animator anim;
    public GameObject scopeIn;
    public Camera weaponCamera;

    bool isScoped = false;

    protected override void Update()
    {
        if (Input.GetMouseButton(0) && ammoCount > 0 && Time.time >= fireRate)
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
            Reload();
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
        yield return new WaitForSeconds(.15f);
        scopeIn.SetActive(isScoped);
        weaponCamera.gameObject.SetActive(!isScoped);
        crosshair.SetActive(!isScoped);
        Camera.main.fieldOfView = defaultFOV / zoom;
    }

    void OnUnscoped()
    {
        scopeIn.SetActive(isScoped);
        weaponCamera.gameObject.SetActive(!isScoped);
        crosshair.SetActive(!isScoped);
        Camera.main.fieldOfView = defaultFOV;
    }
}

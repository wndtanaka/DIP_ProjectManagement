using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Weapon
{
    public float reloadTime;
    public Animator anim;

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

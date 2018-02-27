using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float shootRate = 15f;

    public ParticleSystem muzzleFlash;
    public GameObject projectile;

    float fireRate = 0;
    Camera fpsCam;

    void Start()
    {
        fpsCam = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= fireRate)
        {
            fireRate = Time.time + 1f / shootRate;
            Shoot();
        }
    }
    void Shoot()
    {
        muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Enemy enemy =  hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            GameObject projectileGO =  Instantiate(projectile, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(projectileGO, 2f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject barrel;
    public float strength;
    public float sensitivity;
    public float recoil;
    public GameObject Bullet;
    public int ShootType;
    public float shootingDelay;
    public float reloadTime;
    public int ammoMax;
    public int ammo;
    private float shootingCooldown;
    private GameObject bulletSpawn;
    public float bulletForce;
    public bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        ammo = ammoMax;
    }

    // Update is called once per frame
    void Update()
    {
        //Physics Based Rotation, taken from sword
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        var gunRot = -Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg - gameObject.GetComponent<Rigidbody2D>().rotation + 90;
        if (gunRot < -180)
        {
            gunRot = gunRot + 360;
        }
        else if (gunRot > 180)
        {
            gunRot = gunRot - 360;
        }
        gameObject.GetComponent<Rigidbody2D>().AddTorque(Mathf.Clamp(gunRot, -sensitivity, sensitivity) * 50 * Time.deltaTime * strength);
        //if left click, shoot
        switch (ShootType)
        {
            case 0: //Semi auto fire
                if (Input.GetKeyDown(KeyCode.Mouse0) && ammo > 0 && shootingCooldown <= 0)
                {
                    bulletSpawn = Instantiate(Bullet, barrel.transform.position, barrel.transform.rotation);
                    bulletSpawn.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * bulletForce);
                    barrel.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * recoil);
                    ammo--;
                    shootingCooldown = shootingDelay;
                }
                break;
            case 1: //Full auto fire
                if (Input.GetKey(KeyCode.Mouse0) && ammo > 0 && shootingCooldown <= 0)
                {
                    bulletSpawn = Instantiate(Bullet, barrel.transform.position, barrel.transform.rotation);
                    bulletSpawn.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * bulletForce);
                    barrel.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * recoil);
                    ammo--;
                    shootingCooldown = shootingDelay;
                }
                break;
        }
        //if right click, empty ammo and reload ammo after time passes
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartCoroutine("Reloading");
        }
        //Lower the shooting cooldown
        if (shootingCooldown > 0)
        {
            shootingCooldown = shootingCooldown - Time.deltaTime;
        }
    }
    IEnumerator Reloading()
    {
        reloading = true;
        ammo = 0;
        yield return new WaitForSeconds(reloadTime);
        ammo = ammoMax;
        reloading = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootBlank : MonoBehaviour
{
    //Variables
    public GameObject projectile;
    public Transform[] muzzlePoints;
    public GameObject projectileParent;
    public float projectileLifespan = 2f;
    public float projectileSpeed = 20f;
    public float fireRate = 3f;
    public bool multishotActive = false;
    public bool canFire = true;
    private Coroutine shootCoroutine;

    private void Start()
    {
        projectileParent = GameObject.Find("ProjectileParent");

    }

    // Update is called once per frame
    void Update()
    {
        //check if player hit our shoot button
        if (Input.GetMouseButton(0))
        {
            if (shootCoroutine == null && canFire)
            {
                shootCoroutine = StartCoroutine(DoShoot());
            }
        }

    }

    IEnumerator DoShoot()
    {
        if (!multishotActive)
        {
            //Shoot
            //instantiate our prefab projectile
            GameObject currentProjectile = Instantiate(projectile, muzzlePoints[0].position, muzzlePoints[0].rotation);
            //set the parent of the projectile to a null object so it is not impaced by our character movement
            currentProjectile.transform.SetParent(projectileParent.transform);

            //add force to the projectile
            currentProjectile.GetComponent<Rigidbody>().AddForce(muzzlePoints[0].up * projectileSpeed, ForceMode.Impulse);
            //destroy the projectile after time has passed
            Destroy(currentProjectile, projectileLifespan);

            yield return new WaitForSeconds(fireRate);
            shootCoroutine = null;
        }
        else
        {
            Debug.Log("Multishot fired!");
            foreach (Transform muzzlePoint in muzzlePoints)
            {
                GameObject currentProjectile = Instantiate(projectile, muzzlePoint.position, muzzlePoint.rotation);
                currentProjectile.transform.SetParent(projectileParent.transform);
                currentProjectile.GetComponent<Rigidbody>().AddForce(muzzlePoint.up * projectileSpeed, ForceMode.Impulse);
                Destroy(currentProjectile, projectileLifespan);
            }


            //One muzzle code
            /*
            GameObject currentProjectile1 = Instantiate(projectile, muzzlePoint.position, muzzlePoint.rotation);
            GameObject currentProjectile2 = Instantiate(projectile, new Vector3(muzzlePoint.position.x + 3f, muzzlePoint.position.y, muzzlePoint.position.z), muzzlePoint.rotation);
            GameObject currentProjectile3 = Instantiate(projectile, new Vector3(muzzlePoint.position.x + 3f, muzzlePoint.position.y, muzzlePoint.position.z), muzzlePoint.rotation);

            GameObject[] projectiles = { currentProjectile1, currentProjectile2, currentProjectile3 };

            foreach (GameObject currentProjectile in projectiles)
            {
                currentProjectile.transform.SetParent(projectileParent.transform);
                currentProjectile.GetComponent<Rigidbody>().AddForce(muzzlePoint.up * projectileSpeed, ForceMode.Impulse);
                Destroy(currentProjectile, projectileLifespan);
            }
            */

            yield return new WaitForSeconds(fireRate);
            shootCoroutine = null;
        }



    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtPlayerBlank : MonoBehaviour
{
    //Variable for our look at target
    private Transform _target;
    private GameObject player;
    public GameObject projectile;
    public Transform muzzlePoint;
    public GameObject projectileParent;
    public float projectileLifespan = 10f;
    public float projectileSpeed = 50f;
    public float rateOfFire = 10f;
    private bool isShooting = false;

    private void Start()
    {
        player = GameObject.Find("Player");
        projectileParent = GameObject.Find("ProjectileParent");
    }
    // Update is called once per frame
    void Update()
    {
        //tell gameobject to look at the target
        //_target = player.transform;
        //transform.LookAt(_target);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _target = player.transform;
            transform.LookAt(_target);

            if (!isShooting)
            {
                StartCoroutine(Shoot());
            }

            //InvokeRepeating("shoot", rateOfFire, rateOfFire);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Exited Range");
        }

    }

    IEnumerator Shoot()
    {
        isShooting = true;

        //Shoot at player
        Debug.Log("Turret Fired");

        //instantiate our prefab projectile
        GameObject currentProjectile = Instantiate(projectile, muzzlePoint.position, muzzlePoint.rotation);

        //set the parent of the projectile to a null object so it is not impaced by our character movement
        currentProjectile.transform.SetParent(projectileParent.transform);

        //add force to the projectile
        currentProjectile.GetComponent<Rigidbody>().AddForce(muzzlePoint.up * projectileSpeed, ForceMode.Impulse);

        //destroy the projectile after time has passed
        Destroy(currentProjectile, projectileLifespan);

        yield return new WaitForSeconds(rateOfFire);

        isShooting = false;
    }
}

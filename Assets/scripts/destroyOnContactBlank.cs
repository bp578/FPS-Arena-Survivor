using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyOnContactBlank : MonoBehaviour
{
    //declare our list of strings we'll use if we want our projectile to destroy an object it hits
    public List<string> destroyableObjects = new List<string>();


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            //Destroy projectiles when hitting the ground or walls
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Terrain") || other.gameObject.CompareTag("Destructible"))
        {
            //Destroy projectiles when hitting the ground or walls
            Destroy(gameObject);
        }
    }
}

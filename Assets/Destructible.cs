using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public int currentHP = 5;
    public int maxHP = 5;
    public float respawnTimer = 3f;
    private bool isInvincible = false;
    public MeshRenderer mr;
    public BoxCollider bc;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        bc = GetComponent<BoxCollider>();
    }


    IEnumerator takeDamage(int damage)
    {
        isInvincible = true;
        currentHP -= damage;
        Debug.Log("Destructible HP: " + currentHP);
        //Die
        if (currentHP <= 0)
        {
            mr.enabled = false;
            bc.enabled = false;
            Debug.Log("Destructible destroyed");
            StartCoroutine(respawn(respawnTimer));
        }

        yield return new WaitForSeconds(0.25f);
        isInvincible = false;
    }

    IEnumerator respawn(float time)
    {
        yield return new WaitForSeconds(time);
        mr.enabled = true;
        bc.enabled = true;
        currentHP = maxHP;
        Debug.Log("Destructible respawned");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy projectile") || other.CompareTag("enemy"))
        {
            if (!isInvincible)
            {
                StartCoroutine(takeDamage(1));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            if (!isInvincible)
            {
                StartCoroutine(takeDamage(1));
            }
        }
    }
}

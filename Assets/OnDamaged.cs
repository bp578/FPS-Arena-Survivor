using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamaged : MonoBehaviour
{
    //Enemy game object
    public GameObject self;
    public AudioSource explosion;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerProjectile"))
        {
            explosion.Play();
            Destroy(self);
        }
    }
}

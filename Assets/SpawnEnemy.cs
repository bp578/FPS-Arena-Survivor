using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] enemies;
    public GameObject[] eliteEnemies;
    public float spawnRate = 10f;
    private int eliteTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        InvokeRepeating("spawnEnemy", 3f, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameOver)
        {
            Destroy(this);
        }
    }

    //Every 10 enemies spawned, spawn an elite enemy
    private void spawnEnemy()
    {
        if (eliteTimer == 5 && (eliteEnemies.Length > 0))
        {
            //Spawn random elite enemy
            Debug.Log("ELITE ENEMY SPAWNED");
            int randomIndex = Random.Range(0, eliteEnemies.Length);
            Instantiate(eliteEnemies[randomIndex], this.transform.position, this.transform.rotation);
            eliteTimer = 0;
        }
        else
        {
            //Spawn random basic enemy
            int randomIndex = Random.Range(0, enemies.Length);
            Instantiate(enemies[randomIndex], this.transform.position, this.transform.rotation);
            eliteTimer += 1;
        }

    }
}

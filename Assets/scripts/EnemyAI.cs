using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //Public variables
    public GameObject healthPickup;
    public GameManager gameManager;
    public GameObject enemyParent;
    public GameObject powerup;
    public int pointValue;
    public int HP;
    public float speed;

    //Private variables
    private GameObject player;
    private NavMeshAgent agent;
    private enum State
    {
        patrol, chase
    };
    private State currentState = State.chase;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.chase;
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        enemyParent = GameObject.Find("EnemyParent");
        this.gameObject.transform.SetParent(enemyParent.transform);


    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.chase)
        {
            agent.destination = player.transform.position;
            agent.speed = speed;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("PLAYER IN RANGE: CHASING");
            currentState = State.chase;
        }

        if (other.gameObject.CompareTag("playerProjectile"))
        {
            Debug.Log("SHOT");
            HP -= 1;
            Destroy(other.gameObject);
            if (HP <= 0)
            {
                Die();
            }

        }

    }

    private void Die()
    {

        int randChance = Random.Range(1, 101);

        if (randChance < 102)
        {
            Instantiate(healthPickup, new Vector3(this.transform.position.x, 1, this.transform.position.z), healthPickup.transform.rotation);
        }

        if (powerup != null)
        {
            Instantiate(powerup, new Vector3(this.transform.position.x, 1, this.transform.position.z), powerup.transform.rotation);
        }

        gameManager.updateScore(pointValue);

        Destroy(this.gameObject);
    }

}

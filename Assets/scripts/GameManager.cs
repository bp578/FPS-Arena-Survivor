using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int scoreToWin = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI powerupText;
    public Transform enemies;
    public GameObject player;
    public GameObject levelBoss;
    public FPSControllerBlank playerScript;
    public bool gameOver = false;
    public string nextLevel;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<FPSControllerBlank>();
        enemies = GameObject.Find("EnemyParent").transform;
        playerHealthText = GameObject.Find("Health Display").GetComponent<TextMeshProUGUI>();
        powerupText = GameObject.Find("Powerup Notification").GetComponent<TextMeshProUGUI>();

        if (SceneManager.GetActiveScene().name == "level3")
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            scoreText.text = "Score: " + score + "/" + scoreToWin;
        }

    }

    public void updateScore(int changeInScore)
    {
        score += changeInScore;
        if (SceneManager.GetActiveScene().name == "level3")
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            scoreText.text = "Score: " + score + "/" + scoreToWin;
        }

        if (score >= scoreToWin)
        {
            gameOver = true;

            //Destroy all enemies
            foreach (Transform child in enemies)
            {
                Destroy(child.gameObject);
            }

            scoreText.text = "Defeat the boss";

            if (levelBoss != null)
            {
                Instantiate(levelBoss, this.transform.position, this.transform.rotation);
                levelBoss = null;
            }

        }
    }

    private void Update()
    {
        playerHealthText.text = "HP: " + playerScript.playerHP.ToString();
    }

    public void loadNextLevel(string nextLevel)
    {
        gameOver = true;
        playerScript.resetPosition();
        SceneManager.LoadScene(nextLevel);
    }


}

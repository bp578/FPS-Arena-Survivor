using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    //Player score display
    public TextMeshProUGUI scoreDisplay;

    // Start is called before the first frame update
    void Start()
    {
        scoreDisplay.text = "Destroy all 6 Targets to Win";
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            scoreDisplay.text = "You Win!";
        }
    }

    
}

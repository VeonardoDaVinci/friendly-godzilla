using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : Singleton<ScoreCounter>
{
    private TextMeshProUGUI scoreText;
    void Start()
    {
        scoreText= GetComponent<TextMeshProUGUI>();
        scoreText.text = "0";
    }

    public void ChangeScoreText(string scr)
    {
        scoreText.text = scr;
    }
}

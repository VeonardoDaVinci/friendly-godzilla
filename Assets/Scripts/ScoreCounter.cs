using DG.Tweening;
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

    public void ShakeCounter()
    {
        scoreText.transform.DOShakeScale(0.2f);
    }
}

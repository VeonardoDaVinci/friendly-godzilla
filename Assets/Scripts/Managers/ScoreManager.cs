using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    public int CurrnetScore = 0;
    public int HighestScore = 0;

    public int BuildingsRebuilt = 0;
    public int BuildingDestroyed = 0;
    public TextMeshProUGUI ScoreText;
    public void AddScore(int scr)
    {
        CurrnetScore += scr;
        ScoreText.text = CurrnetScore.ToString();
    }

    public void RemoveScore(int scr)
    {
        CurrnetScore -= scr;
        if(CurrnetScore < 0 )
        {
            CurrnetScore = 0;
        }
        ScoreText.text = CurrnetScore.ToString();
    }
}

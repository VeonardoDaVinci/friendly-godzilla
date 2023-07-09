using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    public int CurrentScore = 0;
    public int HighestScore = 0;

    public int BuildingsRebuilt = 0;
    public int BuildingDestroyed = 0;

    private void Start()
    {
        LevelManager.LevelLoaded += ResetScore;
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        BuildingsRebuilt = 0;
        BuildingDestroyed = 0;
    }
    public IEnumerator RemoveScoreOverTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            RemoveScore(1);
        }
    }
    public void AddScore(int scr)
    {
        CurrentScore += scr;
        ScoreCounter.Instance.ChangeScoreText(CurrentScore.ToString());
    }

    public void RemoveScore(int scr)
    {
        CurrentScore -= scr;
        if(CurrentScore < 0 )
        {
            CurrentScore = 0;
        }
        else
        {
            ScoreCounter.Instance.ChangeScoreText(CurrentScore.ToString());
        }
    }
}

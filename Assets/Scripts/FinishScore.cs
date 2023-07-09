using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buildingScore;
    [SerializeField] TextMeshProUGUI rubbleScore;
    [SerializeField] TextMeshProUGUI completeScore;
    void Start()
    {
        ScoreManager.Instance.ScoreText = FindObjectOfType<TextMeshProUGUI>();
        buildingScore.text = ScoreManager.Instance.BuildingsRebuilt.ToString();
        rubbleScore.text = ScoreManager.Instance.BuildingDestroyed.ToString();
        completeScore.text = ScoreManager.Instance.CurrnetScore.ToString();

    }
}

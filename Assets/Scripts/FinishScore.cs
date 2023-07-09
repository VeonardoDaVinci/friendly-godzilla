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
        buildingScore.text = ScoreManager.Instance.BuildingsRebuilt.ToString();
        rubbleScore.text = ScoreManager.Instance.BuildingDestroyed.ToString();
        completeScore.text = ScoreManager.Instance.CurrentScore.ToString();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum HouseState
{
    Destroyed,
    Prebuilt,
    Rebuilt
}

public class HouseController : MonoBehaviour
{
    public HouseState CurrentHouseState = HouseState.Destroyed;

    public GameObject destroyedVariant;
    public GameObject rebuiltVariant;
    public GameObject prebuiltVariant;

    private void OnEnable()
    {
        switch (CurrentHouseState)
        {
            case HouseState.Destroyed:
                DestroyBuilding();
                break;
            case HouseState.Rebuilt:
                RebuildBuilding();
                break;
            case HouseState.Prebuilt:
                PrebuildBuilding();
                break;
        } 
    }

    private void RebuildBuilding()
    {
        destroyedVariant.SetActive(false);
        rebuiltVariant.SetActive(true);
        prebuiltVariant.SetActive(false);
        CurrentHouseState = HouseState.Rebuilt;
    }

    private void DestroyBuilding()
    {
        destroyedVariant.SetActive(true);
        rebuiltVariant.SetActive(false);
        prebuiltVariant.SetActive(false);
        CurrentHouseState = HouseState.Destroyed;
    }

    private void PrebuildBuilding()
    {
        destroyedVariant.SetActive(false);
        rebuiltVariant.SetActive(false);
        prebuiltVariant.SetActive(true);
        CurrentHouseState = HouseState.Prebuilt;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(CurrentHouseState == HouseState.Rebuilt || CurrentHouseState == HouseState.Prebuilt)
            {
                SoundManager.Instance.PlayRubble();
                Camera.main.transform.DOShakePosition(0.2f,3,50);
                if(CurrentHouseState== HouseState.Rebuilt)
                {
                    PlayerController.Instance.DecreaseHealth(4f);
                }
                else
                {
                    PlayerController.Instance.DecreaseHealth(2f);
                    RandomizationManager.Instance.SpawnNewObject(false);
                }
                DestroyBuilding();
                ScoreManager.Instance.RemoveScore(5);
                ScoreManager.Instance.BuildingDestroyed++;
            }
        }
        if (other.CompareTag("Brick"))
        {
            if (CurrentHouseState == HouseState.Prebuilt)
            {
                RebuildBuilding();
                SoundManager.Instance.PlayRoar();
                Camera.main.transform.DOShakePosition(0.1f, 1, 50);
                RandomizationManager.Instance.ObjectsSpawned.Remove(other.gameObject);
                Destroy(other.gameObject);
                PlayerController.Instance.ChangePlayerHolding(false);
                PlayerController.Instance.IncreaseMaxSpeed();
                RandomizationManager.Instance.SpawnNewObject();

                ScoreManager.Instance.BuildingsRebuilt++;
                ScoreManager.Instance.AddScore(25);
                SoundManager.Instance.PlayScore();
                PlayerController.Instance.IncreasePlayerSize(0.04f);

                if((RandomizationManager.Instance.HousesSpawned.Count-1)%5==0)
                {
                    PlayerController.Instance.IncreasePlayerRange(1f);
                    PlayerController.Instance.IncreaseHealth(2f);
                    RandomizationManager.Instance.IncreaseSpawnRange(1f);
                    DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, Camera.main.orthographicSize+0.5f, 1f);
                    ScoreManager.Instance.AddScore(100);
                    ScoreCounter.Instance.ShakeCounter();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}

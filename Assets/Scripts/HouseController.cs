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

    private void Start()
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
                Camera.main.transform.DOShakePosition(0.2f,3,50);
                DestroyBuilding();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Brick"))
        {
            if (CurrentHouseState == HouseState.Prebuilt)
            {
                RebuildBuilding();
                Camera.main.transform.DOShakePosition(0.1f,1,50);
                Destroy(collision.collider.gameObject);
                PlayerController.Instance.IsHolding = false;
            }
        }
    }
}

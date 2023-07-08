using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizationManager : Singleton<RandomizationManager>
{
    public int SpawnedBuildingCount = 0;
    public List<GameObject> ObjectsSpawned;
    [SerializeField] private List<GameObject> BuildingVariants;
    [SerializeField] private GameObject Brick;
    [SerializeField] private BoxCollider SpawnRange;
    private float SpawnRangeX;
    private float SpawnRangeZ;
    private void Start()
    {
        SpawnRangeX = SpawnRange.size.x / 4f * 0.9f;
        SpawnRangeZ = SpawnRange.size.z / 4f * 0.9f;
        SpawnNewObject();
    }
    public void SpawnNewObject()
    {
        int index = Random.Range(0, BuildingVariants.Count);
        Vector3 newPostition = PositionClosebyRandomBuilding();
        ObjectsSpawned.Add(Instantiate(BuildingVariants[index], newPostition, Quaternion.identity));
        SpawnNewObject(Brick);
        SpawnedBuildingCount++;
    }

    public void SpawnNewObject(GameObject preferedGameObject)
    {
        Vector3 newPostition = RandomPosition();
        GameObject spawnedObject = Instantiate(preferedGameObject, newPostition, Quaternion.identity);
        spawnedObject.transform.localEulerAngles = new Vector3(Random.Range(-15, 15), Random.Range(0, 360), Random.Range(-15, 15));
        ObjectsSpawned.Add(spawnedObject);
    }


    private Vector3 RandomPosition()
    {
        Vector3 newPosition = new Vector3((int)Random.Range(-SpawnRangeX, SpawnRangeX) * 2, 0f, (int)Random.Range(-SpawnRangeZ, SpawnRangeZ) * 2);
        newPosition = CheckForObjectsCloseby(newPosition);
        return newPosition;
    }

    private Vector3 PositionClosebyRandomBuilding()
    {
        Vector3 newPosition = new(0,0, 0);
        int index = Random.Range(0, ObjectsSpawned.Count);
        int direction = Random.Range(0, 4);
        if(ObjectsSpawned.Count<=0)
        {
            return RandomPosition();
        }
        switch (direction)
        {
            case 0:
                newPosition = ObjectsSpawned[index].transform.position + new Vector3(0f,0f,2f);
                break;
            case 1:
                newPosition = ObjectsSpawned[index].transform.position + new Vector3(2f,0f,0f);
                break;
            case 2:
                newPosition = ObjectsSpawned[index].transform.position - new Vector3(0f,0f,2f);
                break;
            case 3:
                newPosition = ObjectsSpawned[index].transform.position - new Vector3(2f,0f,0f);
                break;
        }
        newPosition = CheckForObjectsCloseby(newPosition);
        return newPosition;
    }

    private Vector3 CheckForObjectsCloseby(Vector3 newPostition)
    {
        if (IsToCloseToAnyObject(newPostition))
        {
            newPostition.x = (int)Random.Range(-SpawnRangeX, SpawnRangeX)*2;
            newPostition.y = 0f;
            newPostition.z = (int)Random.Range(-SpawnRangeZ, SpawnRangeZ)*2;
        }
        else
        {
            return newPostition;
        }
        newPostition = CheckForObjectsCloseby(newPostition);
        return newPostition;
    }

    private bool IsToCloseToAnyObject(Vector3 position)
    {
        float margin = 1.5f;
        if (Vector3.Distance(PlayerController.Instance.transform.position, position) <= margin)
        {
            return true;
        }
        foreach (GameObject obj in ObjectsSpawned)
        {
            if (Vector3.Distance(obj.transform.position, position) <= margin)
            {
                return true;
            }
        }
        return false;
    }
}

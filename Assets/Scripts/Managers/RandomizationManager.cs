using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class RandomizationManager : Singleton<RandomizationManager>
{
    public int SpawnedBuildingCount = 0;
    public List<GameObject> ObjectsSpawned;
    public List<GameObject> HousesSpawned;
    [SerializeField] private List<GameObject> BuildingVariants;
    [SerializeField] private GameObject Brick;
    [SerializeField] private BoxCollider SpawnRange;
    private float SpawnRangeX;
    private float SpawnRangeZ;
    private bool anySpaceLeft = true;
    private void Start()
    {
        SpawnRangeX = (SpawnRange.size.x*0.9f) / 2f;
        SpawnRangeZ = (SpawnRange.size.z*0.9f) / 2f;
        SpawnNewObject();
    }
    public void SpawnNewObject()
    {
        if (!anySpaceLeft || ObjectsSpawned.Count>=25) { SceneManager.LoadScene(2); }

        int chance = Random.Range(0, 100);
        int index = Random.Range(0, BuildingVariants.Count);
        GameObject spawnedObject;
        if(chance < 50)
        {
            spawnedObject = PlaceObjectInRandomSpace(index);
            Debug.Log("Random");
        }
        else
        {
            spawnedObject = PlaceObjectCloseByBuilding(index);
            Debug.Log("Nearby");
        }
        anySpaceLeft = CheckIfAnySpaceIsLeft();
        ObjectsSpawned.Add(spawnedObject);
        HousesSpawned.Add(spawnedObject);
        SpawnNewObject(Brick);
        SpawnedBuildingCount++;
    }

    private GameObject PlaceObjectCloseByBuilding(int index)
    {
        GameObject spawnedObject;
        int oldObjectindex = Random.Range(0, HousesSpawned.Count);
        if (HousesSpawned.Count <= 0)
        {
            return PlaceObjectInRandomSpace(index);
        }
        Vector3 newPosition = PositionClosebyBuilding(oldObjectindex);
        if(newPosition == Vector3.zero)
        {
            return PlaceObjectInRandomSpace(index);
            
        }
        spawnedObject = Instantiate(BuildingVariants[index], HousesSpawned[oldObjectindex].transform.parent);
        spawnedObject.transform.localPosition = newPosition;
        //spawnedObject.transform.localEulerAngles = HousesSpawned[oldObjectindex].transform.parent.localEulerAngles;
        return spawnedObject;
    }

    private GameObject PlaceObjectInRandomSpace(int index)
    {
        GameObject spawnedObject;
        Vector3 newPosition = RandomPosition();
        GameObject neighborhood = new GameObject();
        Instantiate(neighborhood);
        neighborhood.transform.position= newPosition;
        spawnedObject = Instantiate(BuildingVariants[index], neighborhood.transform);
        neighborhood.transform.localEulerAngles = RandomRotation();

        return spawnedObject;
        
    }

    public void SpawnNewObject(GameObject preferedGameObject)
    {
        if(!anySpaceLeft) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
        Vector3 newPosition = RandomPosition();
        GameObject spawnedObject = Instantiate(preferedGameObject, newPosition, Quaternion.identity);
        spawnedObject.transform.localEulerAngles = new Vector3(Random.Range(-15, 15), Random.Range(0, 360), Random.Range(-15, 15));
        ObjectsSpawned.Add(spawnedObject);
        anySpaceLeft = CheckIfAnySpaceIsLeft();
    }


    private Vector3 RandomPosition()
    {
        Vector3 newPosition = new Vector3((int)Random.Range(-SpawnRangeX, SpawnRangeX) * 2, 0f, (int)Random.Range(-SpawnRangeZ, SpawnRangeZ) * 2);
        if (CheckForObjectsCloseby(newPosition))
        {
            return RandomPosition();
        }
        return newPosition;
    }

    private Vector3 RandomRotation()
    {
        Vector3 newRotation = new(0f, Random.Range(0, 360), 0f);
        return newRotation;
    }

    private Vector3 PositionClosebyBuilding(int index)
    {
        Vector3 newPosition = new(0,0, 0);
        Vector3 newPositionGlobal = new(0,0,0);
        GameObject helperObject = new();
        int direction = Random.Range(0, 4);
        int directionIndex = direction;
        helperObject = Instantiate(helperObject, HousesSpawned[index].transform.parent);
        for (int i = 0; i < 4;i++)
        {
            directionIndex += i;
            directionIndex %= 4;
            switch (directionIndex)
            {
                case 0:
                    newPosition = HousesSpawned[index].transform.localPosition + new Vector3(0f, 0f, 2f);
                    break;
                case 1:
                    newPosition = HousesSpawned[index].transform.localPosition + new Vector3(2f, 0f, 0f);
                    break;
                case 2:
                    newPosition = HousesSpawned[index].transform.localPosition - new Vector3(0f, 0f, 2f);
                    break;
                case 3:
                    newPosition = HousesSpawned[index].transform.localPosition - new Vector3(2f, 0f, 0f);
                    break;
            }
            helperObject.transform.localPosition = newPosition;
            if (!CheckForObjectsCloseby(helperObject.transform))
            {
                Debug.Log(helperObject.transform.position);
                Debug.Log(newPosition);
                Destroy(helperObject.gameObject);
                return newPosition;
            }
        }

        return Vector3.zero;
    }

    private Vector3 RotationClosebyBuilding(int index)
    {
        Vector3 newRotation = ObjectsSpawned[index].transform.localEulerAngles;
        return newRotation;
    }

    private bool CheckForObjectsCloseby(Vector3 newPosition)
    {
        if (IsTooCloseToAnyObject(newPosition) || Mathf.Abs(newPosition.x) > SpawnRangeX || Mathf.Abs(newPosition.z) > SpawnRangeZ)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckForObjectsCloseby(Transform transform)
    {
        if (IsTooCloseToAnyObject(transform.position) || Mathf.Abs(transform.position.x) > SpawnRangeX || Mathf.Abs(transform.position.z) > SpawnRangeZ)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckIfAnySpaceIsLeft()
    {
        Vector3 checkPosition = new(0f,0f,0f);

        for (int x = -(int)SpawnRangeX; x<=(int)SpawnRangeX; x++)
        {
            for (int z = -(int)SpawnRangeZ; z <= (int)SpawnRangeZ; z++)
            {
                checkPosition.x = x;
                checkPosition.z = z;
                if (!IsTooCloseToAnyObject(checkPosition))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsTooCloseToAnyObject(Vector3 position)
    {
        float margin = 1.5f;
        if (Vector3.Distance(PlayerController.Instance.transform.position, position) <= margin*2)
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

    private bool IsTooCloseToAnyObject(Transform transform)
    {
        float margin = 1.5f;
        if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) <= margin * 2)
        {
            return true;
        }
        foreach (GameObject obj in ObjectsSpawned)
        {
            if (Vector3.Distance(obj.transform.position, transform.position) <= margin)
            {
                return true;
            }
        }
        return false;
    }
}

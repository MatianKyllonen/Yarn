using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    //Area Prefabs
    private List<GameObject> currentSpawnables;
    public List<GameObject> areaPrefabs1;
    public List<GameObject> areaPrefabs2;



    public float areaWidth = 10f; // Width of each area
    public float distanceBetweenAreas = 20f; // Distance between consecutive areas

    private List<GameObject> activeAreas = new List<GameObject>();
    private Transform player;
    private float lastAreaX;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        lastAreaX = player.position.x;
        currentSpawnables = areaPrefabs1;
        GenerateInitialAreas();
    }

    void GenerateInitialAreas()
    {
        while (lastAreaX < player.position.x + distanceBetweenAreas)
        {
            SpawnArea(currentSpawnables);
        }
    }

    void SpawnArea(List<GameObject> prefabList)
    {
        Vector3 spawnPosition = new Vector3(lastAreaX + distanceBetweenAreas, 0, 0f);
        int randomIndex = Random.Range(0, prefabList.Count);
        GameObject newArea = Instantiate(prefabList[randomIndex], spawnPosition, Quaternion.identity);
        activeAreas.Add(newArea);
        lastAreaX = newArea.transform.position.x;
    }

    void Update()
    {
        CheckAreaDistance();

        if (player.transform.position.x >= 150)
        {
            currentSpawnables = areaPrefabs2;
        }
    }

    void CheckAreaDistance()
    {
        if (lastAreaX < player.position.x + distanceBetweenAreas)
        {
            SpawnArea(currentSpawnables);
        }

        for (int i = 0; i < activeAreas.Count; i++)
        {
            if (activeAreas[i].transform.position.x + areaWidth < player.position.x)
            {
                Destroy(activeAreas[i]);
                activeAreas.RemoveAt(i);
            }
        }
    }
}

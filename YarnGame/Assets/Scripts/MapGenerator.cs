using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public List<GameObject> areaPrefabs; // List of prefabs for areas
    public float areaWidth = 10f; // Width of each area
    public float distanceBetweenAreas = 20f; // Distance between consecutive areas

    private List<GameObject> activeAreas = new List<GameObject>();
    private Transform player;
    private float lastAreaX;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        lastAreaX = player.position.x;
        GenerateInitialAreas();
    }

    void GenerateInitialAreas()
    {
        while (lastAreaX < player.position.x + distanceBetweenAreas)
        {
            SpawnArea();
        }
    }

    void SpawnArea()
    {
        Vector3 spawnPosition = new Vector3(lastAreaX + distanceBetweenAreas, 0, 0f);
        int randomIndex = Random.Range(0, areaPrefabs.Count);
        GameObject newArea = Instantiate(areaPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        activeAreas.Add(newArea);
        lastAreaX = newArea.transform.position.x;
    }

    void Update()
    {
        CheckAreaDistance();
    }

    void CheckAreaDistance()
    {
        if (lastAreaX < player.position.x + distanceBetweenAreas)
        {
            SpawnArea();
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

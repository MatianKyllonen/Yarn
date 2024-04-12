using System.Collections.Generic;
using UnityEditor;
using System.Collections;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    //Area Prefabs
    private List<GameObject> currentSpawnables;

    public List<GameObject> areaPrefabs1;
    public List<GameObject> areaPrefabs2;
    public List<GameObject> areaPrefabs3;

    public GameObject transitionArea;

    public float areaWidth = 10f; // Width of each area
    public float distanceBetweenAreas = 20f; // Distance between consecutive areas

    private List<GameObject> activeAreas = new List<GameObject>();
    private Transform player;
    private float lastAreaX;

    private bool transition1Triggered = false;
    private bool transition2Triggered = false;

    public GameObject zone3bg;

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

        if (player.transform.position.x >= 100 && !transition1Triggered)
        {
            Vector3 spawnPosition = new Vector3(lastAreaX + distanceBetweenAreas, 0, 0f);
            GameObject newArea = Instantiate(transitionArea, spawnPosition, Quaternion.identity);
            lastAreaX = newArea.transform.position.x;
            currentSpawnables = areaPrefabs2; 
            transition1Triggered = true; 
        }

        if (player.transform.position.x >= 200 && !transition2Triggered)
        {
            Vector3 spawnPosition = new Vector3(lastAreaX + distanceBetweenAreas, 0, 0f);
            GameObject newArea = Instantiate(transitionArea, spawnPosition, Quaternion.identity);
            lastAreaX = newArea.transform.position.x;
            currentSpawnables = areaPrefabs3;
            transition2Triggered = true;
            StartCoroutine(ChangeBg(zone3bg, 0.5f, 250));
        }
    }

    private IEnumerator ChangeBg(GameObject bg, float delay, float treshhold)
    {
        while (player.transform.position.x < treshhold)
        {
            yield return null;
        }
        yield return new WaitForSeconds(delay);
        bg.SetActive(true);
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
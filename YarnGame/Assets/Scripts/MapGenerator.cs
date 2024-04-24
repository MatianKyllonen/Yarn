using System.Collections.Generic;
using UnityEditor;
using System.Collections;
using UnityEngine;
using TMPro;

public class MapGenerator : MonoBehaviour
{

    //Area Prefabs
    private List<GameObject> currentSpawnables;

    public List<GameObject> areaPrefabs1;
    public List<GameObject> areaPrefabs2;
    public List<GameObject> areaPrefabs3;
    public List<GameObject> areaPrefabs4;

    public GameObject transitionArea;

    public float areaWidth = 10f; // Width of each area
    public float distanceBetweenAreas = 20f; // Distance between consecutive areas

    private List<GameObject> activeAreas = new List<GameObject>();
    private Transform player;
    private float lastAreaX;
    private GameObject lastArea;

    private bool transition1Triggered = false;
    private bool transition2Triggered = false;

    public TextMeshProUGUI zoneText;
    public GameObject zone3bg;
    public GameObject zone4bg;

    private int zoneNum = 1;


    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        lastAreaX = player.position.x;
        currentSpawnables = areaPrefabs1;
    }

    void SpawnArea(List<GameObject> prefabList)
    {

        Vector3 spawnPosition = new Vector3(lastAreaX + distanceBetweenAreas, 0, 0f);
        int randomIndex = Random.Range(0, prefabList.Count);
        GameObject newArea = prefabList[randomIndex];

        // Check if the new area is the same as the last area
        if (newArea == lastArea && lastArea != null)
        {
            // Calculate the index for the previous area (index - 1)
            int previousIndex = (randomIndex + 1) % prefabList.Count;

            newArea = prefabList[previousIndex];
            
        }

        GameObject go = Instantiate(newArea, spawnPosition, Quaternion.identity);
        lastAreaX = go.transform.position.x;

        // Update references
        lastArea = newArea;
        activeAreas.Add(go);
        
    }

    void Update()
    {

        CheckAreaDistance();

        /*if (player.transform.position.x >= 100 && !transition1Triggered)
        {
            Vector3 spawnPosition = new Vector3(lastAreaX + distanceBetweenAreas, 0, 0f);
            GameObject newArea = Instantiate(transitionArea, spawnPosition, Quaternion.identity);
            lastAreaX = newArea.transform.position.x;
            currentSpawnables = areaPrefabs2; 
            transition1Triggered = true;

            StartCoroutine(ChangeBg(null, 0.45f, 150));

        }

        /*if (player.transform.position.x >= 350 && !transition2Triggered)
        {
            Vector3 spawnPosition = new Vector3(lastAreaX + distanceBetweenAreas, 0, 0f);
            GameObject newArea = Instantiate(transitionArea, spawnPosition, Quaternion.identity);
            lastAreaX = newArea.transform.position.x;
            currentSpawnables = areaPrefabs3;
            transition2Triggered = true;
            StartCoroutine(ChangeBg(zone3bg, 0.15f, 400));

        }*/

        if (player.transform.position.x >= 100 && !transition2Triggered)
        {
            Vector3 spawnPosition = new Vector3(lastAreaX + distanceBetweenAreas, 0, 0f);
            GameObject newArea = Instantiate(transitionArea, spawnPosition, Quaternion.identity);
            lastAreaX = newArea.transform.position.x;
            currentSpawnables = areaPrefabs4;
            transition2Triggered = true;
            zone3bg.SetActive(false);
            StartCoroutine(ChangeBg(zone4bg, 0.15f, 150));

        }
    }

    private IEnumerator ChangeBg(GameObject bg, float delay, float treshhold)
    {
        while (player.transform.position.x < treshhold)
        {
            yield return null;
        }
        yield return new WaitForSeconds(delay);

        if (bg != null)
            bg.SetActive(true);

        zoneText.text = ("Zone " + zoneNum);
        zoneNum++;
        zoneText.GetComponent<Animator>().SetTrigger("Mogged");
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
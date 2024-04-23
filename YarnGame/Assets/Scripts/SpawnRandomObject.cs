using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomObject : MonoBehaviour
{

    private List<GameObject> currentSpawnList = new List<GameObject>();

    public List<GameObject> objectsToSpawn1;
    public List<GameObject> objectsToSpawn2;
    public List<GameObject> objectsToSpawn3;

    private GameObject player;

    // Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.Find("Player");

    }
    void Start()
    {


        if (player.transform.position.x > 100)
        {
            print("Swap");
            currentSpawnList.Clear();
            currentSpawnList = objectsToSpawn2;
        }

        if (player.transform.position.x > 350)
        {
            currentSpawnList.Clear();
            currentSpawnList = objectsToSpawn3;
        }
        else
            currentSpawnList = objectsToSpawn1;

        GetComponent<SpriteRenderer>().enabled = false;
        SpawnObject();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }


    void SpawnObject()
    {
        if (currentSpawnList.Count > 0)
        {
            // Choose a random index from the list
            int randomIndex = Random.Range(0, currentSpawnList.Count);

            // Spawn the object at the position of the SpawnRandomObject script
            Instantiate(currentSpawnList[randomIndex], transform.position, Quaternion.identity);
        }
    }
}

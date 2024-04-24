using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomObject : MonoBehaviour
{

    private List<GameObject> currentSpawnList;

    public List<GameObject> objectsToSpawn1;
    public List<GameObject> objectsToSpawn2;
    public List<GameObject> objectsToSpawn3;

    private GameObject player;

    // Start is called before the first frame update

    
    private void Start()
    {

        player = GameObject.Find("Player");

        if (player.transform.position.x > 100 && player.transform.position.x < 350)
        {
            SpawnObject(objectsToSpawn2);
        }
        else if (player.transform.position.x > 350)
        {
            SpawnObject(objectsToSpawn3);
        }
        else
            SpawnObject(objectsToSpawn1);

        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }


    void SpawnObject(List<GameObject> currentSpawnList)
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

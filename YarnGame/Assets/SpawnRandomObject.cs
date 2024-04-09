using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomObject : MonoBehaviour
{
    public List<GameObject> objectsToSpawn; // List to hold the objects you want to spawn

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        SpawnObject();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnObject()
    {
        if (objectsToSpawn.Count > 0)
        {
            // Choose a random index from the list
            int randomIndex = Random.Range(0, objectsToSpawn.Count);

            // Spawn the object at the position of the SpawnRandomObject script
            Instantiate(objectsToSpawn[randomIndex], transform.position, Quaternion.identity);
        }
    }
}

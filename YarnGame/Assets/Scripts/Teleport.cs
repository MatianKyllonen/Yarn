using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    // Start is called before the first frame update


    void Start()
    {
        FindObjectOfType<PlayerMovement>().inMenu = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = new Vector2(0, collision.gameObject.transform.position.y);
        }
    }
}

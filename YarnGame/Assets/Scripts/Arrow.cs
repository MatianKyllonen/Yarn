using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 15f; // Set the speed of the arrow

    // Update is called once per frame
    void Update()
    {
        // Move the arrow to the left with a set velocity
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Destroy the arrow after 2 seconds
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            print(collision.name);
            if (collision.gameObject.tag == "Player")
            {
                GameManager.instance.GameOver();
            }
        }
    }
}

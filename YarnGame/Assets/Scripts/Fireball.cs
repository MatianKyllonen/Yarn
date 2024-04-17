using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 15f; // Set the speed of the arrow

    // Update is called once per frame
    void Update()
    {
        // Move the arrow to the left with a set velocity
        transform.Translate(new Vector3 (-1, -1) * speed * Time.deltaTime);

        // Destroy the arrow after 2 seconds
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameManager.instance.GameOver("Dragon");
            }
        }
    }
}

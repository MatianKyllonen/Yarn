using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy : MonoBehaviour
{
    public float moveDistance = 2.0f; // Total distance the enemy will move
    public float moveSpeed = 2.0f; // Speed of movement

    private float startPosition;

    void Start()
    {
        // Save the initial position of the enemy
        startPosition = transform.position.y;
    }

    void Update()
    {
        // Calculate the horizontal movement using Mathf.PingPong
        float newPositionY = startPosition + Mathf.PingPong(Time.time * moveSpeed, moveDistance) - (moveDistance / 2.0f);

        // Update the position of the game object
        transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.instance.GameOver();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public float moveDistance = 2.0f; // Total distance the enemy will move
    public float moveSpeed = 2.0f; // Speed of movement

    private float startPosition;
    private bool facingRight = true; // To keep track of the facing direction

    void Start()
    {
        startPosition = transform.position.x;
    }

    void Update()
    {
        // This calculates the new position it's gonna go to
        float newPositionX = startPosition + Mathf.PingPong(Time.time * moveSpeed, moveDistance) - (moveDistance / 2.0f);

        // Flip the direction if necessary
        if (newPositionX < transform.position.x && facingRight)
        {
            Flip();
        }
        else if (newPositionX > transform.position.x && !facingRight)
        {
            Flip();
        }

        // This moves towards it on the Y Axis
        transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
    }

    void Flip()
    {
        // Switch the direction the Snake is facing
        facingRight = !facingRight;

        // Multiply the x component of localScale by -1 to flip along the X-axis
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.GameOver("Snake");
        }
    }
}

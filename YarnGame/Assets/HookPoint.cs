using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPoint : MonoBehaviour
{
    private bool playerInRange = false;
    private LineRenderer line;
    private GameObject player;
    private Rigidbody2D playerRigidbody; // Rigidbody component of the player

    public float swingForce = 5f; // Adjust this to control the swing force
    public float swingSpeed = 5f; // Adjust this to control the swing speed

    private float angle = 0f; // Current angle of the swing

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        playerRigidbody = player.GetComponent<Rigidbody2D>(); // Get the Rigidbody component
        line = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKey(KeyCode.Space))
        {
            line.positionCount = 2;
            line.SetPosition(0, player.transform.position);
            line.SetPosition(1, gameObject.transform.position);

            // Update angle based on swing speed
            angle += swingSpeed * Time.deltaTime;

            // Calculate the new position based on the circle equation
            Vector2 circleCenter = transform.position;
            float radius = Vector2.Distance(circleCenter, player.transform.position);
            float x = circleCenter.x + Mathf.Cos(angle) * radius;
            float y = circleCenter.y + Mathf.Sin(angle) * radius;
            Vector2 newPos = new Vector2(x, y);

            // Update player position
            playerRigidbody.MovePosition(newPos);

            // Rotate the player towards the hook point
            Vector2 direction = circleCenter - newPos;
            float angleRadians = Mathf.Atan2(direction.y, direction.x);
            float angleDegrees = angleRadians * Mathf.Rad2Deg;
        }
        else
        {
            line.positionCount = 0;
            angle = 0f; // Reset angle when swinging stops
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}

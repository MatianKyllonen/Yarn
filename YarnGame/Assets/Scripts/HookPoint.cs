using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPoint : MonoBehaviour
{
    private bool playerInRange = false;
    private LineRenderer line;
    private GameObject player;
    private Rigidbody2D playerRb;

    public float swingForce;

    private bool swinging = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        playerRb = player.GetComponent<Rigidbody2D>();
        line = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
            
        if(GameManager.instance != null)
            if (playerInRange && GameManager.instance.dead == false && Input.GetKeyDown(KeyCode.Space) || playerInRange && GameManager.instance.dead == false && Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                GetComponentInChildren<SpriteRenderer>().enabled = false;
                player.GetComponent<PlayerMovement>().swinging = true;
                swinging = true;
            }

        if(GameManager.instance == null)
        {

            if (playerInRange && Input.GetKeyDown(KeyCode.Space) || playerInRange && Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                GetComponentInChildren<SpriteRenderer>().enabled = false;
                player.GetComponent<PlayerMovement>().swinging = true;
                swinging = true;
            }
        }

        if (playerInRange && Input.GetKeyUp(KeyCode.Space) || playerInRange && Input.GetKeyUp(KeyCode.Joystick1Button0))
        {
            player.GetComponent<PlayerMovement>().swinging = false;
            swinging = false;
        }

        if (playerInRange && swinging)
        {

            // Calculate the swing direction
            Vector2 playerToHook = transform.position - player.transform.position;
            float angle = Mathf.Atan2(playerToHook.y, playerToHook.x);

            // Set the swing position to follow a circular trajectory
            float swingRadius = 1f; // Adjust this value as needed
            float swingX = transform.position.x + Mathf.Cos(angle) * swingRadius * Time.deltaTime;
            float swingY = transform.position.y + Mathf.Sin(angle) * swingRadius * Time.deltaTime;

            // Update the swing line renderer
            line.positionCount = 2;
            line.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y + 0.65f));
            line.SetPosition(1,transform.position);

            // Apply swing force
            Vector2 swingForceDirection = new Vector2(swingY - player.transform.position.y, player.transform.position.x - swingX);
            playerRb.AddForce(swingForceDirection.normalized * ((swingForce + playerRb.velocity.magnitude) * 100 + playerRb.gameObject.GetComponent<PlayerMovement>().currentMoveSpeed) * Time.deltaTime);
        }
        else
        {
            line.positionCount = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  
        {
            GetComponentInChildren<SpriteRenderer>().enabled = true;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            player.GetComponent<PlayerMovement>().swinging = false;
            playerInRange = false;
            swinging = false;
        }
    }
}

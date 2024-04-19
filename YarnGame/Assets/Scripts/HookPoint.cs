using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HookPoint : MonoBehaviour
{
    private bool playerInRange = false;
    private LineRenderer line;
    private GameObject player;
    private Rigidbody2D playerRb;

    public float swingForce;
    public float CatchDistance = 4f;

    private bool swinging = false;
    private bool swung = false;

    private Vector2 swingForceDirection;

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
        if (GameManager.instance.player.transform.position.x > transform.position.x)
            playerInRange = false;

        
        if (GameManager.instance != null && !swung)
        {
            float Distance;

            Distance = Vector3.Distance(transform.position,GameManager.instance.player.transform.position);

            if(Distance < CatchDistance && transform.position.y > GameManager.instance.player.transform.position.y)
            {
                GetComponentInChildren<SpriteRenderer>().enabled = true;
                playerInRange = true;
            }

            if (playerInRange && GameManager.instance.player.transform.position.x > transform.position.x + CatchDistance - 1 && swinging)
            {
                print("Range " + playerRb.velocity);
                swung = true;
                GetComponentInChildren<SpriteRenderer>().enabled = false;
                player.GetComponent<PlayerMovement>().swinging = false;         
                playerInRange = false;

                playerRb.AddForce(swingForceDirection.normalized * ((swingForce + playerRb.velocity.magnitude) * 500 + playerRb.gameObject.GetComponent<PlayerMovement>().currentMoveSpeed) * Time.deltaTime);
            }
        }      

        if (playerInRange && GameManager.instance != null && !swung)
        {
            if (GameManager.instance.dead == false)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                    GetComponentInChildren<SpriteRenderer>().enabled = false;
                    player.GetComponent<PlayerMovement>().swinging = true;
                    swinging = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button0))
            {
                print("Release " + playerRb.velocity);
                player.GetComponent<PlayerMovement>().swinging = false;
                swinging = false;
                playerRb.AddForce(swingForceDirection.normalized * ((swingForce + playerRb.velocity.magnitude) * 100 + playerRb.gameObject.GetComponent<PlayerMovement>().currentMoveSpeed) * Time.deltaTime);
            }

            if (swinging)
            {
                // Calculate the swing direction
                Vector2 playerToHook = transform.position - player.transform.position;
                float angle = Mathf.Atan2(playerToHook.y, playerToHook.x) * Mathf.Rad2Deg;

                float maxAngle = 170;

                if (angle > maxAngle)
                {
                    player.GetComponent<PlayerMovement>().swinging = false;
                    swinging = false;
                }
                else
                {
                    float swingRadius = 1f; // Adjust this value as needed
                    float swingX = transform.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * swingRadius * Time.deltaTime;
                    float swingY = transform.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * swingRadius * Time.deltaTime;

                    // Update the swing line renderer
                    line.positionCount = 2;
                    line.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y + 0.65f));
                    line.SetPosition(1, transform.position);

                    // Apply swing force
                    swingForceDirection = new Vector2(swingY - player.transform.position.y, player.transform.position.x - swingX);
                    playerRb.AddForce(swingForceDirection.normalized * ((swingForce + playerRb.velocity.magnitude) * 100 + playerRb.gameObject.GetComponent<PlayerMovement>().currentMoveSpeed) * Time.deltaTime);
                }
            }
            else
            {
                line.positionCount = 0;
            }
        }


    }
}

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
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            swinging = true;
        }

        if (playerInRange && Input.GetKeyUp(KeyCode.Space))
        {
            swinging = false;
        }


        if (playerInRange && swinging)
        {
            line.positionCount = 2;
            line.SetPosition(0, player.transform.position);
            line.SetPosition(1, gameObject.transform.position);

            playerRb.AddForce((gameObject.transform.position - player.transform.position) * swingForce * Time.deltaTime);   

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
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            swinging = false;
        }
    }
}

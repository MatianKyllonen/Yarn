using UnityEngine;

public class HookPoint : MonoBehaviour
{
    private bool playerInRange = false;
    private bool swinging = false;
    private bool swung = false;
    private bool noJumping = true;

    private Vector2 swingForceDirection;

    private GameObject player;
    private Rigidbody2D playerRb;
    private LineRenderer line;

    public float swingForce;
    public float catchDistance = 4f;

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
        checkIfInRange();
        if (!swinging)
        {
            line.positionCount = 0;
        }

        if (playerInRange && !swung)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                if (!player.GetComponent<PlayerMovement>().isGrounded)
                {
                    GetComponentInChildren<SpriteRenderer>().enabled = false;
                    player.GetComponent<PlayerMovement>().swinging = true;
                    swinging = true;
                    noJumping = false;
                }
            }

            if(swinging)
                if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Joystick1Button0))
                {
                    line.positionCount = 0;
                    player.GetComponent<PlayerMovement>().swinging = false;
                    swinging = false;
                    noJumping = true;

                    player.GetComponent<PlayerMovement>().DetachFromRope(2);

                    ApplySwingForce();


                }
        }

        if (swinging)
        {
            ApplySwingForce();
        }


        if (playerInRange && !swung)
        {
            if (GameManager.instance != null && GameManager.instance.dead == false || player.GetComponent<PlayerMovement>().inMenu)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                    GetComponentInChildren<SpriteRenderer>().enabled = false;
                    player.GetComponent<PlayerMovement>().swinging = true;
                    swinging = true;
                }
            }
        }
    }

    private void checkIfInRange()
    {

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < catchDistance && transform.position.y > player.transform.position.y && transform.position.x + 1.5f > player.transform.position.x)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = true;
            playerInRange = true;
        }
        else if (!swinging && !swung && playerInRange)
        {
            swung = true;
            // If not swinging and out of range, stop rendering the hook
            line.positionCount = 0;
            player.GetComponent<PlayerMovement>().swinging = false;
            swinging = false;
            playerInRange = false;
            GetComponentInChildren<SpriteRenderer>().enabled = false;


            if (!noJumping)
            {
                player.GetComponent<PlayerMovement>().DetachFromRope(5);
                ApplySwingForce();
            }

        }
    }

    private void ApplySwingForce()
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
            swingForceDirection = new Vector2(swingY - player.transform.position.y, player.transform.position.x - swingX) * Time.deltaTime;
            playerRb.AddForce(swingForceDirection.normalized * ((swingForce + playerRb.velocity.magnitude) * 100 + playerRb.gameObject.GetComponent<PlayerMovement>().currentMoveSpeed) * Time.deltaTime);
        }
    }
}

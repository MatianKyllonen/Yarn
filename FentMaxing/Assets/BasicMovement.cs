using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public int playerNumber = 1; // Set to 1 or 2 for each player
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float jumpThreshold = 0.5f; // Threshold for joystick vertical input to trigger jump
    public float throwForce = 10f; // Force to throw the object
    public LayerMask pickupLayer; // LayerMask for objects that can be picked up
    public float pickupRadius = 0.5f; // Radius for picking up objects

    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalMove;
    private float verticalMove;
    private GameObject carriedObject;
    private bool isThrowing; // Flag to check if the player is currently throwing an object
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0f, -0.6f), 0.1f, LayerMask.GetMask("Ground"));

        // Get horizontal and vertical input
        horizontalMove = Input.GetAxis("Horizontal_Player" + playerNumber);
        verticalMove = Input.GetAxis("Jump_Player" + playerNumber);

        // If the player is holding an object, make the object face the same direction as the player
        if (carriedObject != null)
        {
            // Determine the scale factor for flipping the object
            float scaleFactor = spriteRenderer.flipX ? -1f : 1f;
            // Adjust the position of the carried object relative to the player
            carriedObject.transform.localPosition = new Vector3(scaleFactor * 1f, 0f, 0f);
        }

        // Apply dead zone
        if (Mathf.Abs(horizontalMove) < 0.1f)
        {
            horizontalMove = 0f;

        }

        // Flip player sprite if moving horizontally
        else if (horizontalMove != 0)
        {
            spriteRenderer.flipX = horizontalMove < 0;
        }

        // Jumping - Triggered by vertical joystick input
        if (verticalMove < -jumpThreshold && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Interact - Pick up and throw object
        if (Input.GetButtonDown("InteractButton" + playerNumber))
        {
            if (carriedObject == null)
            {
                // Pick up object
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius, pickupLayer);
                if (colliders.Length > 0)
                {
                    carriedObject = colliders[0].gameObject;
                    carriedObject.transform.parent = transform;
                    carriedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                }
            }
            else
            {
                // Throw object
                isThrowing = true;
                carriedObject.transform.parent = null;
                carriedObject.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }

        // If the player is throwing an object and the object is not null
        if (isThrowing && carriedObject != null)
        {
            // Calculate the throw direction
            Vector2 throwDirection;
            if (horizontalMove != 0 || !isGrounded)
            {
                // Throw in the direction of movement if moving or not grounded
                throwDirection = new Vector2(horizontalMove, 0.1f).normalized;
            }
            else
            {
                // Throw in the direction the player is facing if standing still and grounded
                throwDirection = spriteRenderer.flipX ? new Vector2(-1, 0.1f).normalized : new Vector2(1, 0.1f).normalized;
            }

            // Apply throw force in the calculated direction
            carriedObject.GetComponent<Rigidbody2D>().velocity = throwDirection * throwForce;

            // Reset throwing flag and carried object
            isThrowing = false;
            carriedObject = null;
        }
    }


    public void TakeDamage()
    {
        if (playerNumber == 1)
        {
            GameManager.gm.playerAlive1 = false;
        }
        else if (playerNumber == 2)
        {
            GameManager.gm.playerAlive2 = false;
        }

        GameManager.gm.EndGame();

        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        // Move the player
        rb.velocity = new Vector2(horizontalMove * moveSpeed, rb.velocity.y);
    }
}

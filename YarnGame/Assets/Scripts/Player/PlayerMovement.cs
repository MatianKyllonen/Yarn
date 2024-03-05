using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    private float moveInput = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Handle player movement
        if (Input.GetKey(right)){

            moveInput = 1;
        }
        else if (Input.GetKey(left))
        {

            moveInput = -1;
        }
        else
            moveInput = 0;

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);


        // Jumping
        if (isGrounded && Input.GetKeyDown(up))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
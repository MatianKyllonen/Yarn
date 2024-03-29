using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    public SpriteRenderer rope;


    public KeyCode up;
    public KeyCode down;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            moveSpeed = 7;
        }
        else
            animator.SetBool("isJumping", true);

        if (Input.GetKey(down))
        {
            rb.velocity = new Vector2(1 * moveSpeed, rb.velocity.y - 0.05f);
        }

        if (transform.position.y < 0)
            GameManager.instance.GameOverFadeOut();
            

        rb.velocity = new Vector2(1 * moveSpeed, rb.velocity.y);


        // Jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {          
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }



}
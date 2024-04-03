using System.Collections;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    public float currentMoveSpeed = 7f;
    public float baseMoveSpeed = 7;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    public SpriteRenderer rope;
    private bool groundPounding;
    public GameObject groundPoundParticle;


    public KeyCode up;
    public KeyCode down;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(AddSpeed());
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.65f, groundLayer);

        // Check if the raycast hits something
        if (hit.collider != null)
        {
                // Player is grounded
                isGrounded = true;
        }
        else
         isGrounded= false;




        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            currentMoveSpeed = baseMoveSpeed;

            if(groundPounding)
            {
                animator.SetBool("isGroundPounding", false);
                groundPounding = false;
                print("Pl�ts");
                Instantiate(groundPoundParticle, groundCheck.transform.position, Quaternion.identity);
            }
        }
        else
            animator.SetBool("isJumping", true);

        if (Input.GetKey(down) && !isGrounded)
        {
            rb.velocity = new Vector2(1 * currentMoveSpeed, rb.velocity.y - 0.05f);
            groundPounding = true;
            animator.SetBool("isGroundPounding", true);
        }
        else
        {
            groundPounding = false;
            animator.SetBool("isGroundPounding", false);
        }
            

        if (transform.position.y < 0)
            GameManager.instance.GameOverFadeOut();
            

        rb.velocity = new Vector2(1 * currentMoveSpeed, rb.velocity.y);


        // Jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {          
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    IEnumerator AddSpeed()
    {
        yield return new WaitForSeconds(3);
        baseMoveSpeed += 0.1f;
        currentMoveSpeed = baseMoveSpeed;
        StartCoroutine(AddSpeed());
    }



}
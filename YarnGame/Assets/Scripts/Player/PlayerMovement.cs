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
    private bool isSliding = false;

    public bool swinging;


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



        //Reset values when on ground
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            currentMoveSpeed = baseMoveSpeed;

            if(groundPounding)
            {
                animator.SetBool("isGroundPounding", false);
                groundPounding = false;
                print("Pläts");
                Instantiate(groundPoundParticle, groundCheck.transform.position, Quaternion.identity);
            }
        }
        else
            animator.SetBool("isJumping", true);

        //Ground Pound
        if (Input.GetKey(down) && !isGrounded)
        {
            rb.velocity = new Vector2(1 * currentMoveSpeed, rb.velocity.y - 50f * Time.deltaTime);
            groundPounding = true;
            animator.SetBool("isGroundPounding", true);
        }
        else
        {
            groundPounding = false;
            animator.SetBool("isGroundPounding", false);
        }

        //Sliding
        if (Input.GetKey(down) && isGrounded && !isSliding)
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            isSliding = true;
            animator.SetBool("isSliding", true);
            baseMoveSpeed += 2;

        }

        if(Input.GetKeyUp(down) && isSliding)
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            isSliding = false;
            animator.SetBool("isSliding", false);
            baseMoveSpeed -= 2;
        }


        if (transform.position.y < 0)
            GameManager.instance.GameOverFadeOut();
            
        if(!swinging)
            rb.velocity = new Vector2(1 * currentMoveSpeed, rb.velocity.y);
        else
        {
            rb.velocity = Vector2.zero;
        }


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



    public void EndSlide()
    {

    }



}
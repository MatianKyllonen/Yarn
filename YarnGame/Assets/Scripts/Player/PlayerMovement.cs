using System.Collections;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    public float currentMoveSpeed = 7f;
    public float baseMoveSpeed = 7;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public Animator animator;
    public bool inMenu;

    private Rigidbody2D rb;
    private bool isGrounded;
    public SpriteRenderer rope;
    [HideInInspector] public bool groundPounding;
    private bool isSliding = false;

    [HideInInspector]  public bool swinging;
    

    [Header("Particles")]
    public GameObject groundPoundParticle;
    public GameObject slideParticle;
    private GameObject particle;

    [Header("Binds")]
    public KeyCode up;
    public KeyCode down;

    public AudioClip jumpSfx;
    public AudioClip poundSfx;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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
            isGrounded = false;



        //Reset values when on ground
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            currentMoveSpeed = baseMoveSpeed + (gameObject.transform.position.x / 200);

            if (groundPounding && !isSliding)
            {
                GetComponent<AudioSource>().PlayOneShot(poundSfx, 0.3f);
                animator.SetBool("isGroundPounding", false);
                groundPounding = false;
                Instantiate(groundPoundParticle, groundCheck.transform.position, Quaternion.identity);
            }
        }
        else
            animator.SetBool("isJumping", true);

        //Ground Pound
        if (Input.GetKey(down) && !isGrounded || Input.GetAxisRaw("Vertical") < 0 && !isGrounded)
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
        if (Input.GetKey(down) && isGrounded && !isSliding || Input.GetAxisRaw("Vertical") < 0 && isGrounded && !isSliding)
        {
            particle = Instantiate(slideParticle, transform.position, Quaternion.identity);
            particle.transform.parent = transform;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
            isSliding = true;
            animator.SetBool("isSliding", true);
            baseMoveSpeed += 1.5f;

        }

        if (Input.GetKeyUp(down) && isSliding || Input.GetAxisRaw("Vertical") >= 0 && isSliding)
        {
            Destroy(particle);
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            isSliding = false;
            animator.SetBool("isSliding", false);
            baseMoveSpeed -= 1.5f;
        }


        if (transform.position.y < -2 && !inMenu)
            GameManager.instance.GameOverFadeOut();

        if (!swinging)
            rb.velocity = new Vector2(1 * currentMoveSpeed, rb.velocity.y);
        else
        {
            rb.velocity = Vector2.zero;
        }



        // Jumping
        if (isGrounded && (Input.GetKeyDown(KeyCode.JoystickButton0) || isGrounded && Input.GetKeyDown(KeyCode.Space)) || isGrounded && Input.GetAxis("Vertical") > 0)
        {
            GetComponent<AudioSource>().PlayOneShot(jumpSfx, 0.6f);
            // Set initial jump velocity
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2f - 1) * Time.deltaTime;
        }

       
    }
 }
    


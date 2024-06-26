using System;
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

    private bool launching;

    private Rigidbody2D rb;
    public bool isGrounded;
    public SpriteRenderer rope;
    public bool groundPounding;
    private bool isSliding = false;

    [HideInInspector]  public bool swinging;

    private bool spedUp = false;

    [Header("Particles")]
    public GameObject groundPoundParticle;
    public GameObject slideParticle;
    private GameObject particle;


    private bool checkingSpeed;

    [Header("Binds")]
    public KeyCode up;
    public KeyCode down;

    public AudioClip jumpSfx;
    public AudioClip poundSfx;

    private bool jumping;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();     
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
            baseMoveSpeed += 1;

        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, groundLayer);

        // Check if the raycast hits something
        if (hit.collider != null)
        {
            // Player is grounded
            isGrounded = true;
        }
        else
            isGrounded = false;


        if (transform.position.y < -2 && !inMenu)
        {
            if (transform.position.x > 400 && transform.position.x < 700)
                GameManager.instance.GameOverFadeOut("Water");

            else if (transform.position.x > 700)
                GameManager.instance.GameOverFadeOut("Lava");
            else
                GameManager.instance.GameOverFadeOut("Void");

        }
    }

    private void Update()
    {

        Inputs();


        //Reset values when on ground
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            currentMoveSpeed = baseMoveSpeed + (gameObject.transform.position.x / 250);

            if (groundPounding)
            {
                GetComponent<AudioSource>().PlayOneShot(poundSfx, 0.3f);
                animator.SetBool("isGroundPounding", false);
                groundPounding = false;
                Instantiate(groundPoundParticle, new Vector2(groundCheck.transform.position.x + 1.5f, groundCheck.transform.position.y), Quaternion.identity);
            }
        }
        else
            animator.SetBool("isJumping", true);



        if (!swinging)
            rb.velocity = new Vector2(1 * currentMoveSpeed, rb.velocity.y);
        else
        {
            rb.velocity = Vector2.zero;
        }




       
    }

    private void Inputs()
    {
        // Jumping
        if (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.Space) || Input.GetAxisRaw("Vertical") > 0)
        {
            if (isGrounded && !jumping)
            {
                jumping = true;

                GetComponent<AudioSource>().PlayOneShot(jumpSfx, 0.6f);
                // Set initial jump velocity
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        if (rb.velocity.y < 0)
        {
            jumping = false;
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2f - 1) * Time.deltaTime;
        }

        //Ground Pound
        if(!isGrounded && !launching)
            if (Input.GetKey(down) || Input.GetAxisRaw("Vertical") < 0)
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

            if (!spedUp)
            {
                baseMoveSpeed += 1.75f;
                spedUp = true;
            }

        }

        if (Input.GetKeyUp(down) && isSliding || Input.GetAxisRaw("Vertical") >= 0 && isSliding)
        {
            Destroy(particle);
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            isSliding = false;

            animator.SetBool("isSliding", false);

            if (!checkingSpeed)
                StartCoroutine(DelaySlideSpeedChange());
        }
    }

    private IEnumerator DelaySlideSpeedChange()
    {
        checkingSpeed = true;

        yield return new WaitForSeconds(0.25f);

        if (!isSliding)
        {
            spedUp = false;
            baseMoveSpeed -= 1.75f;
        }

        checkingSpeed = false;

    }

    public void DetachFromRope(float force)
    {
        print("BUST BOY BUST");
        rb.velocity = Vector2.zero;

        rb.velocity = new Vector2(rb.velocity.x * 100, force * 100) * Time.deltaTime;
    }

    public void JumpLaunch()
    {
        launching = true;
        if (groundPounding)
            rb.velocity = new Vector2(rb.velocity.x, 12);
        else
            rb.velocity = new Vector2(rb.velocity.x, 10);
        StartCoroutine(ResetLaunch());
    }

    private IEnumerator ResetLaunch()
    {
        yield return new WaitForSeconds(0.25f);
        launching = false;
    }

}
    


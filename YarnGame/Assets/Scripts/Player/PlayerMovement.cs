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


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(!inMenu) 
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
            baseMoveSpeed += 2;

        }

        if(Input.GetKeyUp(down) && isSliding || Input.GetAxisRaw("Vertical") >= 0 && isSliding)
        {
            Destroy(particle);
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            isSliding = false;
            animator.SetBool("isSliding", false);
            baseMoveSpeed -= 2;
        }


        if (transform.position.y < 0 && !inMenu)
            GameManager.instance.GameOverFadeOut();
            
        if(!swinging)
            rb.velocity = new Vector2(1 * currentMoveSpeed, rb.velocity.y);
        else
        {
            rb.velocity = Vector2.zero;
        }


        // Jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.JoystickButton0) || isGrounded && Input.GetKeyDown(KeyCode.Space))
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
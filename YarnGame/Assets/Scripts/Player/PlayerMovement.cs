using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;


    public float ropeRange = 3f;
    private bool hasWhipped;
    private Rigidbody2D rb;
    private bool isGrounded;

    public KeyCode up;
    public KeyCode down;

    private LineRenderer line;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        line = GetComponentInChildren<LineRenderer>();
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if(isGrounded)
            hasWhipped = false;

        rb.velocity = new Vector2(1 * moveSpeed, rb.velocity.y);

        if (!isGrounded && Input.GetKey(down) && Input.GetKeyDown(KeyCode.Space) && !hasWhipped)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, ropeRange);

            if (hit.transform != null)
                if (hit.transform.gameObject.tag == "Ground")
                {
                    line.positionCount = 2; // Set position count to 1
                    line.SetPosition(0, transform.position);
                    line.SetPosition(1, transform.position);
                    StartCoroutine(DrawRope(hit.point));

                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
                else
                {
                    print("Rope jump");
                    line.positionCount = 2; // Set position count to 1
                    line.SetPosition(0, transform.position);
                    line.SetPosition(1, transform.position);
                    StartCoroutine(DrawRope(new Vector3(transform.position.x, transform.position.y - ropeRange)));
                }
            else
            {
                print("Rope jump");
                line.positionCount = 2; // Set position count to 1
                line.SetPosition(0, transform.position);
                line.SetPosition(1, transform.position);
                StartCoroutine(DrawRope(new Vector3(transform.position.x, transform.position.y - ropeRange)));
            }

            hasWhipped = true;

        }

        // Jumping
        if (isGrounded && Input.GetKeyDown(up))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    IEnumerator DrawRope(Vector3 targetPos)
    {

            Vector3 startPos = line.GetPosition(0); 
            line.positionCount = 2; 

            float duration = 0.1f; 
            float elapsedTime = 0f; 

            while (elapsedTime < duration)
            {

                startPos = line.GetPosition(0);
                elapsedTime += Time.deltaTime; // Update elapsed time
                float t = Mathf.Clamp01(elapsedTime / duration); // Calculate interpolation parameter

                // Interpolate between the start and target positions
                Vector3 newPos = Vector3.Lerp(startPos, targetPos, t);

                // Update the position of the rope
                line.SetPosition(1, newPos);

                yield return null; // Wait for the next frame
            }

            // Ensure the rope reaches the target position
            line.SetPosition(1, targetPos);

            // Wait for a short delay before hiding the rope
            yield return new WaitForSeconds(0.3f);

            // Hide the rope by setting its position count to 0
            line.positionCount = 0;

            
    }

}
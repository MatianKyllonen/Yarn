using UnityEngine;

public class Ball : MonoBehaviour
{
    public string playerTag; // Tag for detecting players
    public LayerMask groundLayer; // LayerMask for detecting ground

    private bool isGrounded;

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(0f, -0f), 0.5f, LayerMask.GetMask("Ground"));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGrounded && collision.gameObject.CompareTag(playerTag))
        {
            // Check if the collided object has a Rigidbody2D and is not kinematic
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            if (rb != null && !rb.isKinematic)
            {
                // Apply damage to the player
                BasicMovement playerMovement = collision.gameObject.GetComponent<BasicMovement>();
                if (playerMovement != null)
                {
                    playerMovement.TakeDamage();
                }
            }
        }
    }
}

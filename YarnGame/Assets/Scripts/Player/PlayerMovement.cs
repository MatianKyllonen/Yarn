using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    public float moveSpeed = 7f;

    [Header("Dash")]
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float dashCoolDown = 0.75f;
    public LineRenderer dashLineRenderer; // Reference to the LineRenderer in the scene
    private bool canDash = true;
    private bool isDashing = false;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, moveY);

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        movement *= moveSpeed * Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && !isDashing && canDash)
        {
            StartCoroutine(Dash(movement));
        }

        if (!isDashing)
        {
            transform.position += movement;
        }
    }

    IEnumerator Dash(Vector3 movement)
    {
        canDash = false;
        isDashing = true;
        Vector3 dashStartPosition = transform.position;
        Vector3 dashTargetPosition = transform.position + (movement * (dashDistance * 100));
        Debug.DrawLine(dashStartPosition, dashTargetPosition);
        Debug.Log(dashStartPosition + " " + dashTargetPosition);
        float startTime = Time.time;

        // Set initial positions for the line renderer
        dashLineRenderer.SetPosition(0, dashStartPosition);
        dashLineRenderer.SetPosition(1, dashStartPosition);
        dashLineRenderer.enabled = true; // Make the line renderer visible

        while (Time.time < startTime + dashDuration)
        {
            float t = (Time.time - startTime) / dashDuration;
            transform.position = Vector3.Lerp(dashStartPosition, dashTargetPosition, t);

            // Update the line renderer position
            dashLineRenderer.SetPosition(0, dashStartPosition);
            dashLineRenderer.SetPosition(1, transform.position);

            // Damage enemies along the path
            DamageEnemiesInPath(dashStartPosition, transform.position);

            yield return null;
        }

        transform.position = dashTargetPosition;
        isDashing = false;
        dashLineRenderer.enabled = false; // Hide the line renderer after the dash

        StartCoroutine(DashRecharge(dashCoolDown));
    }

    void DamageEnemiesInPath(Vector3 startPos, Vector3 endPos)
    {
        Vector3 direction = (endPos - startPos).normalized;
        float distance = Vector3.Distance(startPos, endPos);

        RaycastHit[] hits = Physics.RaycastAll(startPos, direction, distance);

        /*foreach (RaycastHit hit in hits)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
        }*/
    }

    IEnumerator DashRecharge(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }
}

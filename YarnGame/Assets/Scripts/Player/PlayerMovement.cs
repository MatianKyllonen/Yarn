using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    public float moveSpeed = 7f;

    [Header("Dash")]
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float dashCoolDown = 0.75f;
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
        isDashing = true;
        Vector3 dashStartPosition = transform.position;
        Vector3 dashTargetPosition = transform.position += movement * (dashDistance * 100);
        Debug.DrawLine(dashStartPosition, dashTargetPosition);
        Debug.Log(dashStartPosition + " " + dashTargetPosition);
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            float t = (Time.time - startTime) / dashDuration;
            transform.position = Vector3.Lerp(dashStartPosition, dashTargetPosition, t);
            yield return null;
        }

        transform.position = dashTargetPosition;
        isDashing = false;

        StartCoroutine(DashRecharge(dashCoolDown));
    }

    IEnumerator DashRecharge(float cooldown)
    {
        canDash = false;
        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }
}

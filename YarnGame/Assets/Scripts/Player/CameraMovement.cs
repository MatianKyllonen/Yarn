using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // Reference to the player's Transform
    public float smoothing = 0.1f; // Adjust this value for desired smoothness

    void LateUpdate()
    {
        // Calculate the target position with the same Z-coordinate as the camera
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Interpolate smoothly between the current camera position and the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}

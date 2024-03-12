using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; 
    public float smoothing = 0.1f; 
    public float offset = 0;

    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x + offset, target.position.y + 2, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}
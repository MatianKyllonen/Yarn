using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; 
    public float smoothing = 0.1f; 
    public float offset = 0;

    private float yOffset = 2;

    void FixedUpdate()
    {
        if (target.position.y > 10)
            yOffset = -3;
        else
            yOffset = 2;


        Vector3 targetPosition = new Vector3(target.position.x + offset, target.position.y + yOffset, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; 
    public float smoothing = 0.1f; 
    public float offset = 0;

    public bool following;
    private float yOffset = 2;

    void FixedUpdate()
    {

        if (following)
        {
            float targetYOffset;

            if (target.position.y > 10)
                targetYOffset = -3;
            else
                targetYOffset = 2;

            // Smoothly interpolate between the current yOffset and the targetYOffset
            yOffset = Mathf.Lerp(yOffset, targetYOffset, 2.5f * Time.deltaTime);

            Vector3 targetPosition = new Vector3(target.position.x + offset, target.position.y + yOffset, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        }

    }


}
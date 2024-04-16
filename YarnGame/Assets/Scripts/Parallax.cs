using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] backgrounds;     // Array of all the backgrounds to be parallaxed
    private float[] parallaxScales;     // The proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;        // How smooth the parallax will be

    private Transform cam;              // Reference to the main camera's transform
    private Vector3 previousCamPos;     // The position of the camera in the previous frame

    // Called before Start(). Great for references.
    void Awake()
    {
        // Set up the camera reference
        cam = Camera.main.transform;
        Debug.Log("AAA");
        Debug.Log("AAA");
        Debug.Log("AAA");

    }

    // Called on startup
    void Start()
    {
        // The previous frame had the current frame's camera position
        previousCamPos = cam.position;

        // Assigning corresponding parallax scales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("AAA");
        // For each background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // The parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            // Set a target x position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // Keep the current y position
            float backgroundTargetPosY = backgrounds[i].position.y;

            // Create a target position which is the background's current position with its target x position and same y position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);

            // Fade between current position and target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

            // Check if the background has moved beyond the threshold
            Debug.Log((cam.position.x - backgrounds[i].position.x));

            if ((cam.position.x - backgrounds[i].position.x) >= 10f) // Adjust the threshold as needed
            {
                // If it has, reset its position to the opposite side
                backgrounds[i].position = new Vector3(cam.position.x, backgrounds[i].position.y, backgrounds[i].position.z);
            }
        }

        // Set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }



}
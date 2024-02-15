using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public float pointSpacing = 1f; // Distance between points
    public LineRenderer lineRenderer;

    private List<Vector3> pathPoints = new List<Vector3>();
    private Transform player; // Reference to the player's transform
    private bool isSpacePressed = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    void Update()
    {
        // Check if the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpacePressed = true;
        }

        // Check if the space key is released
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isSpacePressed = false;
            ClearPath();
        }

        // Spawn a new point behind the player while space is held down
        if (isSpacePressed)
        {
            Vector3 newPoint = player.position - player.forward * pointSpacing;
            pathPoints.Add(newPoint);

            // Update the Line Renderer
            lineRenderer.positionCount = pathPoints.Count;
            lineRenderer.SetPositions(pathPoints.ToArray());
        }
    }

    void ClearPath()
    {
        pathPoints.Clear();
        lineRenderer.positionCount = 0;
    }
}

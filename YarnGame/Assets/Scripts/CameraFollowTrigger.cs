using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTrigger : MonoBehaviour
{

    private CameraMovement cS;

    private void Start()
    {
        cS = Camera.main.GetComponent<CameraMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        cS.following = true;
    }
}

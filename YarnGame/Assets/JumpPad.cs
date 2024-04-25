using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class JumpPad : MonoBehaviour
{

    private Animator animator;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            Launch();
        }
    }

    private void Launch()
    {
        animator.SetTrigger("Activate");
        player.GetComponent<PlayerMovement>().JumpLaunch();
    }
}

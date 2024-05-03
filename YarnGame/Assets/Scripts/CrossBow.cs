using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBow : MonoBehaviour
{
    public GameObject Arrow;
    private bool fired = false;
    public LayerMask playerMask;

    public AudioClip shootSfx;

    private GameObject player;

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 15f, playerMask);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Player" && !fired)
            {
                player = hit.collider.gameObject;
                fired = true;
                gameObject.GetComponentInChildren<Animator>().SetTrigger("Fire");
            }

        }
    }

    public void ShootArrow()
    {
        player.GetComponent<AudioSource>().PlayOneShot(shootSfx);
        Vector3 spawnPos = new Vector2(transform.position.x - 0.5f, transform.position.y);
        Instantiate(Arrow, spawnPos, Quaternion.identity);    
    }

     
    
}

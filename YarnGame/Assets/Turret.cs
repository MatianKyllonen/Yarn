using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public AudioClip fireSfx;
    public GameObject fireBall;

    private bool inRange;
    private GameObject player;

    public void ShootFireBall()
    {
        if(inRange)
        {
            player.GetComponent<AudioSource>().PlayOneShot(fireSfx, 0.25f);
            Vector3 spawnPos = new Vector2(transform.position.x - 0.5f, transform.position.y);

            Instantiate(fireBall, spawnPos, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            inRange = true;
        }
    }
}

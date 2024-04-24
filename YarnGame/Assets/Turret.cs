using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public AudioClip fireSfx;
    public GameObject fireBall;

    public void ShootFireBall()
    {
        GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(fireSfx, 0.25f);
        Vector3 spawnPos = new Vector2(transform.position.x - 0.5f, transform.position.y);

        Instantiate(fireBall, spawnPos, Quaternion.identity);

    }
}

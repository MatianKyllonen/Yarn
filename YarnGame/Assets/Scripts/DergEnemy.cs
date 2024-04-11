using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DergEnemy : MonoBehaviour
{
    public float moveDistance = 2.0f; 
    public float moveSpeed = 2.0f;

    public GameObject fireBall;
    public GameObject fireBallDown;

    private float startPosition;
    private bool shooting;

    void Start()
    {
        startPosition = transform.position.y;
        StartCoroutine(FireBallLoop());
    }

    void Update()
    {

        if (!shooting)
        {
            float newPositionY = startPosition + Mathf.PingPong(Time.time * moveSpeed, moveDistance) - (moveDistance / 2.0f);

            transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
        }
    }

    IEnumerator FireBallLoop()
    {
        yield return new WaitForSeconds(0.75f);
        shooting = true;
        GetComponent<Animator>().SetTrigger("Fire");
    }

    public void shootFireBall()
    {
        shooting = false;
        Vector3 spawnPos = new Vector2(transform.position.x - 0.5f, transform.position.y);
        int x = Random.Range(0, 2);
        if(x == 1)
        {
            Instantiate(fireBall, spawnPos, Quaternion.identity);
        }
        else
            Instantiate(fireBallDown, spawnPos, Quaternion.identity);

        StartCoroutine(FireBallLoop());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.GameOver("Dragon");
        }
    }
}

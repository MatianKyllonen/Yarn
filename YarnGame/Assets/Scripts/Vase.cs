using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : MonoBehaviour
{
    public GameObject[] vaseSprites; // Array to hold different sprite variations
    public GameObject coinPrefab;
    private Animator currentAnimator;

    private void Start()
    {
        // Randomly choose a sprite variation and activate it
        int randomIndex = Random.Range(0, vaseSprites.Length);
        for (int i = 0; i < vaseSprites.Length; i++)
        {
            vaseSprites[i].SetActive(i == randomIndex);
            if (i == randomIndex)
            {
                currentAnimator = vaseSprites[i].GetComponent<Animator>();
            }
        }

        if(GameManager.instance != null)
            Destroy(gameObject, 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.GetComponent<PlayerMovement>().groundPounding)
            {
                currentAnimator.SetTrigger("Broken");

                if (Random.Range(0f, 1f) > 0.8f)
                {
                    StartCoroutine(SpawnCoins());
                }
            }
        }
    }

    IEnumerator SpawnCoins()
    {
        GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        // Set the velocity of the coin to shoot upward at a random 75-degree angle
        float angle = Random.Range(9.5f, 15.5f); // Angle range from 9.5 to 15.5 degrees
        Vector2 randomDirection = Quaternion.Euler(0, 0, angle) * Vector2.up;
        coin.GetComponent<Rigidbody2D>().velocity = randomDirection * 15;
        // Activate the coin's collider after a delay
        yield return new WaitForSeconds(0.1f);
        coin.GetComponent<CircleCollider2D>().enabled = true;
    }
}

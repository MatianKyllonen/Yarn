using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    private SpriteRenderer sr;
    public Sprite openChest;
    public GameObject coinPrefab;
    public float coinSpeed = 5f;
    public int numberOfCoins = 3;
    private GameObject player;

    public AudioClip coinSpawnSfx;

    private void Start()
    {
        Destroy(gameObject, 10);

        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.GetComponent<PlayerMovement>().groundPounding)
            {
                player = collision.gameObject;

                sr.sprite = openChest;

                StartCoroutine(SpawnCoins());
            }
        }
    }

    IEnumerator SpawnCoins()
    {
        

        for (int i = 0; i < Random.Range(2,5); i++)
        {

            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            // Set the velocity of the coin to shoot upward at a random 75-degree angle
            float angle = Random.Range(9.5f, 15.5f); // Angle range from 7.5 to 10.5 degrees
            Vector2 randomDirection = Quaternion.Euler(0, 0, angle) * Vector2.up;
            coin.GetComponent<Rigidbody2D>().velocity = randomDirection * coinSpeed;
            // Activate the coin's collider after a delay
            yield return new WaitForSeconds(0.075f);

            player.GetComponent<AudioSource>().PlayOneShot(coinSpawnSfx, 0.3f);

            coin.GetComponent<CircleCollider2D>().enabled = true;

        }

        
    }

}


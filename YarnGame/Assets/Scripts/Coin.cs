using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private Animator currentAnimator;
    public GameObject[] coinSprites;
    private bool collected;

    public AudioClip pickupSound;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        // Randomly choose a sprite variation and activate it
        int randomIndex = Random.Range(0, coinSprites.Length);
        for (int i = 0; i < coinSprites.Length; i++)
        {
            coinSprites[i].SetActive(i == randomIndex);
            if (i == randomIndex)
            {
                currentAnimator = coinSprites[i].GetComponent<Animator>();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            currentAnimator.SetTrigger("Pickup");
            if (!collected)
            {
                collision.GetComponent<AudioSource>().PlayOneShot(pickupSound);
                GameManager.instance.coins += currentAnimator.gameObject.GetComponent<CoinValue>().coinValue;
            }


            collected = true;
            Destroy(gameObject, 0.2f);
        }
    }


}

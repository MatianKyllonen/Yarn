using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponentInChildren<Animator>().SetTrigger("Pickup");
            GameManager.instance.score += 10;
            Destroy(gameObject, 0.2f);
        }
    }
}

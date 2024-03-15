using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    private PlayerMovement playerScript;
    public float fireForce = 25;


    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("FIRE");
            playerScript = collision.GetComponent<PlayerMovement>();
            playerScript.moveSpeed = 0;
            collision.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.GetComponent<Rigidbody2D>().AddForce(new Vector3(fireForce, fireForce * 20, 0));
            StartCoroutine(addForceLater());
        }
    }

    IEnumerator addForceLater()
    {
        yield return new WaitForSeconds(0.1f);
        playerScript.moveSpeed = fireForce;
    }
}

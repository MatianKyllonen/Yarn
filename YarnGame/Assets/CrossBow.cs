using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBow : MonoBehaviour
{
    public GameObject Arrow;

    IEnumerator ShootArrow()
    {
        yield return new WaitForSeconds(1);
        print("PEW");
        Instantiate(Arrow, transform.position, Quaternion.identity);    
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShootArrow());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") { }
            StartCoroutine(ShootArrow());
    }
}

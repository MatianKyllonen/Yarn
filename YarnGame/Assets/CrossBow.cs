using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBow : MonoBehaviour
{
    public GameObject Arrow;

    IEnumerator ShootArrow()
    {
        yield return new WaitForSeconds(2);
        Instantiate(Arrow);
        ShootArrow();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShootArrow();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleMagnet : MonoBehaviour
{
    public float attractionSpeed = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            StartCoroutine(MoveTowardsAttractor(other.transform));
        }
    }

    IEnumerator MoveTowardsAttractor(Transform collectible)
    {
        Vector3 initialPosition = collectible.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            if(collectible == null)
                break;

            elapsedTime += Time.deltaTime * attractionSpeed;
            collectible.position = Vector3.Lerp(initialPosition, transform.position, elapsedTime);
            yield return null;
        }
    }
}

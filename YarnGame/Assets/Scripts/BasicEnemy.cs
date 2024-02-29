using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public GameObject player;
    public float idleRange = 1.0f;
    public float chargeSpeed = 10f;
    public float chargeRange = 5f;

    public float moveSpeed = 4f;

    private bool charging = false;

    enum EnemyState { Moving, Charging }
    EnemyState currentState = EnemyState.Moving;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Check if the player is within charge range
        if (distance <= chargeRange)
        {
            currentState = EnemyState.Charging;

            Vector3 direction = (player.transform.position - transform.position).normalized;
        }
        else
        {
            currentState = EnemyState.Moving;
        }

        // Movement based on state
        switch (currentState)
        {
            case EnemyState.Moving:
                // Move towards the player
                Vector3 direction = (player.transform.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
                break;
            case EnemyState.Charging:

                StartCoroutine(Charge());

                
                if(charging)
                {
                    transform.Translate(direction * chargeSpeed * Time.deltaTime);
                }
             break;



        }
    }

    private IEnumerator Charge()
    {
        yield return new WaitForSeconds(1f);
  
        charging = true;

        yield return new WaitForSeconds(1f);

        charging = false;




    }

}

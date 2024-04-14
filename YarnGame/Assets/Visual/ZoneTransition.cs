using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTransition : MonoBehaviour
{

    [SerializeField] private GameObject transitionFade;
    bool busting;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (!busting)
            {
                transitionFade.GetComponent<Rigidbody2D>().AddForce(Vector3.left * 3000);
                busting = true;
            }
        }
    }
    
    
}

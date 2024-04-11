using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mogged : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.dead == true)
        {
            GetComponent<Animator>().SetTrigger("Mogged");
        }
    }
}

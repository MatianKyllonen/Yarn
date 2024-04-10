using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nyooooom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Bust();
    }
            
    public void Bust()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector3.left * 3000);
    }
}

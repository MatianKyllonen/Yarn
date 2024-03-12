using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI distanceText;
    private GameObject player;
    private int score;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        score = Mathf.RoundToInt(player.transform.position.x);

        distanceText.text = (score.ToString() + "0");
    }
}

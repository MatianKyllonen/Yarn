using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI distanceText;
    private GameObject player;
    public int score;
    public int coins;
    private int distance;
    private bool fading;

    public GameObject deathParticle; 
    public GameObject fadeOutImage;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timerText;

    public bool dead;

    private float s;
    private int m;

    public EnemyDatabase enemyDatabase;
    [SerializeField] public Image killerImage;

    [Header("Game Over Section")]
    public GameObject gameOverScreen;
    public TextMeshProUGUI killerText;

    public TextMeshProUGUI gocoinsText;
    public TextMeshProUGUI gotimerText;
    public TextMeshProUGUI goDistanceText;
    



    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Mathf.RoundToInt(player.transform.position.x);

        if (!dead)
        {
            s += 1f * Time.deltaTime;

            if(s >= 60)
            {
                m += 1;
                s = 0;
            }



            if (m != 0)
                if (s < 10)
                    timerText.text = (m.ToString() + ":" + "0" + s.ToString("F2"));
                else
                    timerText.text = (m.ToString() + ":" + s.ToString("F2"));
            else
                    timerText.text = (s.ToString("F2"));


            if (Input.GetKeyDown(KeyCode.V))
                s += 10;


            distanceText.text = (distance.ToString() + "0" + " M");
            coinsText.text = (coins.ToString() + "0");
            
        }
            

        if (fading)
        {
            float newY = fadeOutImage.transform.position.y + 2000 * Time.deltaTime;
            fadeOutImage.transform.position = new Vector3(fadeOutImage.transform.position.x, newY, fadeOutImage.transform.position.z);
        }
    }

    public void GameOver(string killer)
    {

        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<Rigidbody2D>().isKinematic = true;

        if (!dead)
        {
            
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GameObject deathEffect = Instantiate(deathParticle, player.transform);
            deathEffect.transform.position = new Vector3(deathEffect.transform.position.x, deathEffect.transform.position.y - 1);
            player.GetComponentInChildren<SpriteRenderer>().enabled = false;
            StartCoroutine(ShowScore(killer));

        }

            
    }

    public void GameOverFadeOut()
    {
        if (!dead)
            fading = true;

        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(ShowScore("Void"));
    }



    IEnumerator ShowScore(string killer)
    {
        dead = true;

        goDistanceText.text = distanceText.text;
        gocoinsText.text = coinsText.text;
        gotimerText.text = timerText.text;

        Sprite killerSprite = GetKillerSprite(killer);
        if (killerSprite != null)
        {
            // Set the sprite image
            // Assuming you have a UI Image component for displaying the sprite
            killerImage.sprite = killerSprite;
        }

        yield return new WaitForSeconds(1);
        gameOverScreen.SetActive(true);
        killerText.text = killer;      
    }

    private Sprite GetKillerSprite(string killerName)
    {
        foreach (var enemyInfo in enemyDatabase.enemyInfoList)
        {
            if (enemyInfo.enemyType.ToString() == killerName)
            {
                return enemyInfo.picture;
            }
        }
        // If no match found, return a default sprite or handle it as needed
        return null;
    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject.Find("Fade").GetComponent<Nyooooom>().Bust();
    }
    
}

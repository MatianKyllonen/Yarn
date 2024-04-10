using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public bool dead;

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
            distanceText.text = (distance.ToString() + "0");
            coinsText.text = (coins.ToString() + "0");
        }
            

        if (fading)
        {
            float newY = fadeOutImage.transform.position.y + 2000 * Time.deltaTime;
            fadeOutImage.transform.position = new Vector3(fadeOutImage.transform.position.x, newY, fadeOutImage.transform.position.z);
        }
    }

    public void GameOver()
    {

        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<Rigidbody2D>().isKinematic = true;

        if (!dead)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GameObject deathEffect = Instantiate(deathParticle, player.transform);
            deathEffect.transform.position = new Vector3(deathEffect.transform.position.x, deathEffect.transform.position.y - 1);
            player.GetComponentInChildren<SpriteRenderer>().enabled = false;
            StartCoroutine(ShowScore());

        }

            
    }

    public void GameOverFadeOut()
    {
        if (!dead)
            fading = true;

        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(ShowScore());
    }



    IEnumerator ShowScore()
    {
        dead = true;
        score += distance;
        distanceText.text = score.ToString() + "0";
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject.Find("Fade").GetComponent<Nyooooom>().Bust();
    }
    
}

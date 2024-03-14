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
    private int distance;

    public GameObject fadeOutImage;

    private bool dead;

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

        if(!dead)
            distanceText.text = (distance.ToString() + "0");
    }

    public void GameOver()
    {

        player.GetComponent<PlayerMovement>().enabled = false;      
        if (!dead)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            StartCoroutine(ShowScore());
        }

            
    }

    public void GameOverFadeOut()
    {
        if (!dead)
            StartCoroutine(FadeOut());
        GameOver();
    }


    IEnumerator FadeOut()
    {
        while (fadeOutImage.transform.position.y <= 450)
        {
            yield return new WaitForSeconds(0.05f);
            print(fadeOutImage.transform.position.y);
            float newY = fadeOutImage.transform.position.y + 1000 * Time.deltaTime;
            fadeOutImage.transform.position = new Vector3(fadeOutImage.transform.position.x, newY, fadeOutImage.transform.position.z);
        }
    }


    IEnumerator ShowScore()
    {
        dead = true;
        score += distance;
        distanceText.text = score.ToString() + "0";
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

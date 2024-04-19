using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI distanceText;
    [HideInInspector]
    public GameObject player;
    public int score;
    public float coins;
    private float distance;
    private bool fading;

    public GameObject deathParticle;
    public GameObject fadeOutImage;
    public GameObject black;

    public AudioClip hitSound;

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
    public TextMeshProUGUI victimsText;
    public TextMeshProUGUI gotimerText;
    public TextMeshProUGUI goDistanceText;

    private Dictionary<string, int> enemyKills = new Dictionary<string, int>(); // Track kills for each enemy type

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        player = FindObjectOfType<PlayerMovement>().gameObject;

        // Load kills for each enemy type from PlayerPrefs
        foreach (var enemyInfo in enemyDatabase.enemyInfoList)
        {
            string enemyName = enemyInfo.enemyType.ToString();
            int kills = PlayerPrefs.GetInt(enemyName + "_Kills", 0);
            enemyKills[enemyName] = kills;
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance = Mathf.RoundToInt(player.transform.position.x);

        if (!dead)
        {
            s += 1f * Time.deltaTime;

            if (s >= 60)
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
            coinsText.text = (coins.ToString());
        }

        if (fading)
        {
            float newY = fadeOutImage.transform.position.y + 2000 * Time.deltaTime;
            fadeOutImage.transform.position = new Vector3(fadeOutImage.transform.position.x, newY, fadeOutImage.transform.position.z);
        }
    }

    public void GameOver(string killer)
    {
        if (dead)
            return;

        player.GetComponent<AudioSource>().PlayOneShot(hitSound);

        // Increment kills for the specific enemy type
        if (enemyKills.ContainsKey(killer))
            enemyKills[killer]++;
        else
            enemyKills[killer] = 1;

        // Save kills for each enemy type to PlayerPrefs
        foreach (var enemyInfo in enemyDatabase.enemyInfoList)
        {
            string enemyName = enemyInfo.enemyType.ToString();
            PlayerPrefs.SetInt(enemyName + "_Kills", enemyKills[enemyName]);
        }
        PlayerPrefs.Save();

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
        StartCoroutine(ILoveBlackMenShakingTheirCheeks());
        if (!dead)
            fading = true;

        player.GetComponent<PlayerMovement>().enabled = false;

        if (enemyKills.ContainsKey("Void"))
            enemyKills["Void"]++;
        else
            enemyKills["Void"] = 1;

        // Save kills for each enemy type to PlayerPrefs
        foreach (var enemyInfo in enemyDatabase.enemyInfoList)
        {
            string enemyName = enemyInfo.enemyType.ToString();
            PlayerPrefs.SetInt(enemyName + "_Kills", enemyKills[enemyName]);
        }

        PlayerPrefs.Save();

        StartCoroutine(ShowScore("Void"));

    }

    IEnumerator ILoveBlackMenShakingTheirCheeks()
    {
        yield return new WaitForSeconds(2);
        black.SetActive(true);
    }

    IEnumerator ShowScore(string killer)
    {
        dead = true;

        goDistanceText.text = distanceText.text;
        gocoinsText.text = coinsText.text;
        gotimerText.text = timerText.text;

        int kills = 0;
        if (enemyKills.ContainsKey(killer))
        {
            kills = enemyKills[killer];
        }
        victimsText.text = "Victims: " + kills.ToString();

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

        EventSystem.current.SetSelectedGameObject(GameObject.Find("RestartButton"));

        score = Mathf.RoundToInt((distance + coins) - float.Parse(gotimerText.text));

        if (IsTopScore(score))
        {
            // Save the score if it's among the top 5
            SaveScore(score);
        }
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

    private bool IsTopScore(int distance)
    {
        // Load existing top scores from PlayerPrefs
        List<int> topScores = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            int record = PlayerPrefs.GetInt("TopScore_" + i);
            topScores.Add(record);
        }

        // Check if the distance is greater than any of the top scores
        foreach (int record in topScores)
        {
            if (score > record)
            {
                return true;
            }
        }

        return false;
    }

    private void SaveScore(int score)
    {
        // Load existing top scores from PlayerPrefs
        List<int> topScores = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            int record = PlayerPrefs.GetInt("TopScore_" + i);
            topScores.Add(record);
        }

        // Check where the new score fits in the top scores list
        int insertIndex = -1;
        for (int i = 0; i < topScores.Count; i++)
        {
            if (score > topScores[i])
            {
                insertIndex = i;
                break;
            }
        }

        // If the new score is among the top 5, insert it into the list
        if (insertIndex != -1)
        {
            topScores.Insert(insertIndex, score);
            // Remove the lowest score if the list exceeds 5 scores
            if (topScores.Count > 5)
            {
                topScores.RemoveAt(5);
            }

            // Save the updated top scores back to PlayerPrefs
            for (int i = 0; i < topScores.Count; i++)
            {
                PlayerPrefs.SetInt("TopScore_" + i, topScores[i]);
            }
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject.Find("Fade").GetComponent<Nyooooom>().Bust();
    }
}

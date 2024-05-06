using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool playerAlive1 = true;
    public bool playerAlive2 = true;

    public static int player1Score;
    public static int player2Score;

    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    public GameObject endScreen;

    public static GameManager gm;

    private void Start()
    {
        if (gm == null)
            gm = this;

        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void EndGame()
    {
        endScreen.SetActive(true);

        if (playerAlive2 == false)
        {
            player1Score += 1;
            player1ScoreText.text = player1Score.ToString();
}
        else if (playerAlive1 == false)
        {
            player2Score += 1;
            player2ScoreText.text = player2Score.ToString();
        }

        print(playerAlive1 + " " + playerAlive2);

        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{

    private GameObject fade;
    private GameObject leaderBoard;

    private bool triggerd;
    // Enum defining different button types

    private void Start()
    {

        leaderBoard = GameObject.Find("LeaderBoard");
        fade = GameObject.Find("Fade");

    }
    public enum ButtonType
    {
        Start,
        LeaderBoard,
        Quit
    };

    // Action associated with each button type
    [System.Serializable]
    public class ButtonAction
    {
        public ButtonType type;
    }

    public ButtonAction[] buttonActions; // Array of button actions


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.JoystickButton0) && triggerd)
        {
            ShowBoard();
            triggerd = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.GetComponent<PlayerMovement>().groundPounding)
            {
                // Check each button action
                foreach (ButtonAction action in buttonActions)
                {
                    GetComponent<Animator>().SetTrigger("Pushed");
                    // If the button type matches, perform the associated action
                    if (action.type == ButtonType.Start)
                    {
                        StartCoroutine(StartGame());
                        fade.GetComponent<Rigidbody2D>().AddForce(Vector3.left * 3000);
                    }
                    else if (action.type == ButtonType.Quit)
                    {

                        StartCoroutine(QuitGame());
                        fade.GetComponent<Rigidbody2D>().AddForce(Vector3.left * 3000);
                    }
                    else if (action.type == ButtonType.LeaderBoard)
                    {
                        ShowBoard();
                    }
                    // Add additional else if statements for other button types here
                }
            }
        }
    }


    // Methods for different actions
    private IEnumerator StartGame()
    {

        Debug.Log("Starting the game...");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
        // Add your code to start the game here
    }



    private IEnumerator QuitGame()
    {
        Debug.Log("Quitting the game...");
        yield return new WaitForSeconds(1);
        Application.Quit();
    }

    private void ShowBoard()
    {     
        leaderBoard.GetComponent<Animator>().SetTrigger("Trigger");
    }
}

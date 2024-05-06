using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Button"));
    }
    public void GoGame()
    {
        SceneManager.LoadScene(0);
    }

    public void NoGame()
    {
        Application.Quit();
    }
}

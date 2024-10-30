using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button loadGameBtn;

    public void NewGame()
    {
        SceneManager.LoadScene("MainBase");
    }

    public void ExitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}

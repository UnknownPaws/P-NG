using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject titleScreen, settingsScreen;

    private void Start()
    {
        settingsScreen.SetActive(false);
    }


    public void quit()
    {
        Application.Quit();
    }

    public void startGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void settings()
    {
        titleScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void back()
    {
        titleScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }
}

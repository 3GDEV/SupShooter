using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject controlMenu;

    private string lastMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton();
        }
    }

    public void StartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ControlButton()
    {
        lastMenu = "controlMenu";
        mainMenu.SetActive(false);
        controlMenu.SetActive(true);
    }

    public void BackButton()
    {
        if (lastMenu == "controlMenu")
        {
            controlMenu.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
            lastMenu = null;
        }
    }

}

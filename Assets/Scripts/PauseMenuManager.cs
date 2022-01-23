using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenuManager : MonoBehaviour
{
    [Space]

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject gameOver;
    public GameObject hud;
    public float gameOverFadeInDuration = 2;

    [Space]

    [Header("Cursor")]
    public Texture2D cursorTexture;

    [Space]

    [Header("Audio")]
    public AudioMixer masterAudioMixer;


    private bool gameIsPaused = false;
    private string lastMenu;
    
    private PlayerManager player;

    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (gameIsPaused)
            {
                BackButton();
            }
            else
            {
                Pause();
            }
        }

    }
    public void Pause()
    {
        hud.SetActive(false);
        pauseMenu.SetActive(true);
        player.SetAllowFire(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    // PAUSE MENU BUTTONS

    public void ResumeButton()
    {
        hud.SetActive(true);
        pauseMenu.SetActive(false);
        player.SetAllowFire(true);
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), CursorMode.Auto);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void OptionsButton()
    {
        lastMenu = "pauseMenu";
        pauseMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(true);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }

    // OPTIONS MENU BUTTONS

    public void BackButton()
    {
        if (lastMenu == "pauseMenu")
        {
            optionsMenu.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
            lastMenu = string.Empty;
        }
        else
        {
            ResumeButton();
        }
    }

    // GAME OVER MENU BUTTONS
    public void RestartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void SetVolume(float volume)
    {
        masterAudioMixer.SetFloat("MasterVolume", volume);
    }

    public void ToggleGameOver()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        hud.SetActive(false);
        gameOver.SetActive(true);
    }

}

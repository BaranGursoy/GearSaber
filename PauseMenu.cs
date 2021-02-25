using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;


    private void Start()
    {
        pauseMenuUI.SetActive(false); // It is called when the game starts. This line prevents activating pause menu
    }

    void Update()
    {
        if(!isGamePaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }


    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Stop the game time when pause menu is activated
        isGamePaused = true;
    }

    public void LoadMenu()
    {
        pauseMenuUI.SetActive(false);

        if (Score.score > PlayerPrefs.GetInt("HighScore", 0)) // If score is bigger than high score, change the high score
        {
            PlayerPrefs.SetInt("HighScore", Score.score);
        }

        GearAndGameControls.deathCounter++; // Even if you don't die going back to menu should trigger the ads
        SceneManager.LoadScene("MainMenu");
    }


    public void ButtonAudio()
    {
        FindObjectOfType<PauseMenuAudioManager>().Play("Menu");
    }

    public void MenuButtonAudio()
    {
       DontDestroyOnLoad(FindObjectOfType<PauseMenuAudioManager>());
       FindObjectOfType<PauseMenuAudioManager>().Play("Menu");
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelControl : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject levelEndPanel;
    public int levelNum;
    private int nextLevelNum;
    private int hasAccessToLevel;
    private static bool isPaused = false;
    public GameObject notOpened;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused && pausePanel.activeInHierarchy == false)
            {
                PauseButton();
            }
            else if (isPaused && pausePanel.activeInHierarchy == true)
            {
                ContinueButton();
            }
        }
    }
    private void Start()
    {
        levelNum = SceneManager.GetActiveScene().buildIndex;
        nextLevelNum++;
        if(notOpened != null)
        notOpened.SetActive(false);
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(nextLevelNum);
    }

    public void PauseButton()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ContinueButton()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void RestartButton()
    {
        isPaused = false;
        notOpened.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }
    public void ToMainMenu(int scene)
    {
        isPaused = false;
        notOpened.SetActive(false);
        SceneManager.LoadScene(scene);
        Time.timeScale = 1f;
    }

    public void ToNextLevel()
    {
        hasAccessToLevel = PlayerPrefs.GetInt("Level" + (levelNum + 1) + "Access");
        if (hasAccessToLevel == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            if (levelEndPanel != null)
                levelEndPanel.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            notOpened.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ResetStats()
    {
        PlayerPrefs.DeleteAll();
    }
    public void CheatFuels()
    {
        int fuels = PlayerPrefs.GetInt("fuels");
        PlayerPrefs.SetInt("fuels", fuels + 100);
    }
}

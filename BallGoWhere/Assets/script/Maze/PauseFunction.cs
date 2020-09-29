using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseFunction : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pausePanel, PauseButton;
    public void pauseGame()
    {
        pausePanel.SetActive(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0f;
        
    }

    public void ResumeGame()
    {

        pausePanel.SetActive(false);
        PauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public void resetLevel()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        GameController.instance.EndGame();
    }

    public void exitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
        GameController.instance.EndGame();
    }

    public void nextGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameController.instance.EndGame();
    }

    
}

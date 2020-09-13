using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   

    public void GamePlay()
    {
        Debug.Log("[Play] button clicketty click! Accessing MainMenu.cs");
        //Load the next scene
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }


    public void QuitGame()
    {
        Debug.Log("[Quit] button clicketty click! Accessing MainMenu.cs");
        Application.Quit();
    }
}

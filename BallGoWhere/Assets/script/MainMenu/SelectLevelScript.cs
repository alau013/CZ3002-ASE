using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class SelectLevelScript : MonoBehaviour
{
    public GameObject MenuScreen;
    public GameObject SelectScreen;
    public GameObject HelpScreen;
    private bool helpToggle = false;

    public void ToBack()
    {
        MenuScreen.SetActive(true);
        SelectScreen.SetActive(false);
    }

    public void PlayLevel(string scene_name)
    {
        SelectScreen.SetActive(false);
        SceneManager.LoadScene(scene_name);
    }
    public void ToHelp()
    {
        HelpScreen.SetActive(!helpToggle);
        helpToggle = !helpToggle;
    }
}

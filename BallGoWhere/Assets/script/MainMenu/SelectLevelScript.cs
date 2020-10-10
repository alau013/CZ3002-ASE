using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class SelectLevelScript : MonoBehaviour
{
    public Button TypeButton;
    public GameObject SpecialScrollView;
    public GameObject StandardScrollView; 
    public GameObject APIObject;
    public GameObject PrefObject;
    public GameObject MenuScreen;
    public GameObject SelectScreen;
    public GameObject HelpScreen;
    private bool helpToggle = false;
    private int vizMode = 0; //0 - Standard, 1 - Special
    private int numModes = 2;

    public void OnEnable()
    {
        if(vizMode == 0)
        {
            TypeButton.GetComponentInChildren<TMP_Text>().text = "Standard";
            StandardScrollView.SetActive(true);
            SpecialScrollView.SetActive(false);

        }
        else
        {
            TypeButton.GetComponentInChildren<TMP_Text>().text = "Special";
            StandardScrollView.SetActive(false);
            SpecialScrollView.SetActive(true);
        }
    }
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

    public void togglePrevMode()
    {
        vizMode += 1;
        vizMode %= numModes;
        OnEnable();
    }

    public void toggleNextMode()
    {
        vizMode -= 1;
        vizMode %= numModes;
        OnEnable();
    }
}

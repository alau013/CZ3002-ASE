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
    private PlayerPrefUI playerInfo;
    private Transform scrollView;
    private Transform viewPort;
    private Transform entryContainer;
    private LoginResponseAPI loginData = new LoginResponseAPI();

    public void OnEnable()
    {
        playerInfo = PrefObject.GetComponent<PlayerPrefUI>();
        playerInfo.LoadDataFromPlayerPref(LoginMenu.playerName);
        int level = playerInfo.Data.level;

        if (vizMode == 0)
        {
            APIScript AccessAPI = APIObject.GetComponent<APIScript>();
            ArrayList arr = AccessAPI.PostLogin(LoginMenu.playerName);
            if (!arr[0].Equals("ERROR") && !arr[0].Equals("INVALID"))
            { 
                //ignore
            }
            else
            {
                loginData = (LoginResponseAPI)arr[1];
                playerInfo.Data.LoadLoginData(loginData);
                playerInfo.SaveDataToPlayerPref();

                level = playerInfo.Data.level;
            }
            
            TypeButton.GetComponentInChildren<TMP_Text>().text = "Standard";
            StandardScrollView.SetActive(true);
            SpecialScrollView.SetActive(false);

            if (transform.Find("Scroll View") != null)
            {
                scrollView = transform.Find("Scroll View");
                if (scrollView.Find("Viewport") != null)
                {
                    viewPort = scrollView.Find("Viewport");
                    if (viewPort.Find("Content") != null)
                    {
                        entryContainer = viewPort.Find("Content");
                        //int numLevels = entryContainer.childCount;
                        Debug.Log("[level]: " + level);
                        if (level < 4)
                        {
                            entryContainer.Find("LevelFourButton").GetComponent<Button>().interactable = false;
                            entryContainer.Find("LevelFiveButton").GetComponent<Button>().interactable = false;
                            entryContainer.Find("LevelSixButton").GetComponent<Button>().interactable = false;
                        }
                        else
                        {
                            entryContainer.Find("LevelFourButton").GetComponent<Button>().interactable = true;
                            entryContainer.Find("LevelFiveButton").GetComponent<Button>().interactable = true;
                            entryContainer.Find("LevelSixButton").GetComponent<Button>().interactable = true;
                        }
                        if (level < 7)
                        {
                            entryContainer.Find("LevelSevenButton").GetComponent<Button>().interactable = false;
                            entryContainer.Find("LevelEightButton").GetComponent<Button>().interactable = false;
                            entryContainer.Find("LevelNineButton").GetComponent<Button>().interactable = false;
                        }
                        else
                        {
                            entryContainer.Find("LevelSevenButton").GetComponent<Button>().interactable = true;
                            entryContainer.Find("LevelEightButton").GetComponent<Button>().interactable = true;
                            entryContainer.Find("LevelNineButton").GetComponent<Button>().interactable = true;
                        }
                        if (level < 10)
                        {
                            entryContainer.Find("LevelTenButton").GetComponent<Button>().interactable = false;
                            entryContainer.Find("LevelElevenButton").GetComponent<Button>().interactable = false;
                            entryContainer.Find("LevelTwelveButton").GetComponent<Button>().interactable = false;
                        }
                        else
                        {
                            entryContainer.Find("LevelTenButton").GetComponent<Button>().interactable = true;
                            entryContainer.Find("LevelElevenButton").GetComponent<Button>().interactable = true;
                            entryContainer.Find("LevelTwelveButton").GetComponent<Button>().interactable = true;
                        }

                    }
                }
            }
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

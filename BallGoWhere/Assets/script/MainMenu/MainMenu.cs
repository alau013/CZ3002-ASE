using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    public TMP_Text display;
    public GameObject APIObject;
    public GameObject PrefObject;
    public GameObject MenuScreen;
    public GameObject SelectScreen;
    public GameObject LeaderboardsScreen;
    public GameObject ChallengesScreen;
    public GameObject DashboardScreen;
    public GameObject LoginScreen;
    public GameObject HelpScreen;
    private bool helpToggle = false;
    private PlayerPrefUI playerInfo;

    public void OnEnable()
    {
        playerInfo = PrefObject.GetComponent<PlayerPrefUI>();
        playerInfo.LoadDataFromPlayerPref(LoginMenu.playerName);
        //Ensure that display message is set to player's name when active.
        if (display != null)
        {
            
            display.text = "Welcome " + playerInfo.Data.getUsername() + "!";
        }
    }

    public void ToSelect()
    {
        //Disable this screen enable other screen.
        Debug.Log("[Play] button clicketty click! Accessing MainMenu.cs");
        MenuScreen.SetActive(false);
        SelectScreen.SetActive(true);
        

    }

    public void ToLeaderboards()
    {
        MenuScreen.SetActive(false);
        LeaderboardsScreen.SetActive(true);
    }

    public void ToDashboard()
    {
        MenuScreen.SetActive(false);
        DashboardScreen.SetActive(true);
    }

    public void ToChallenges()
    {
        MenuScreen.SetActive(false);
        ChallengesScreen.SetActive(true);
    }

    public void ToLogOut()
    {
        LoginMenu.LogOut();
        MenuScreen.SetActive(false);
        LoginScreen.SetActive(true);
    }
    public void QuitGame() //NOT USING
    {
        Debug.Log("[Quit] button clicketty click! Accessing MainMenu.cs");
        Application.Quit();
    }

    public void ToHelp()
    {
        HelpScreen.SetActive(!helpToggle);
        helpToggle = !helpToggle;
    }
}

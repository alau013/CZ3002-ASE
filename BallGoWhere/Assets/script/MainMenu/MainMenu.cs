using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

/*
 * This class implements MainMenu.cs This provides the logic to handle the interactions of the user with the user interface.
 * This allows the user to view and select the features of the application.
 *
 * @author Tay Jaslyn
 * 
 */

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

            PlayerPrefs.SetString("user",playerInfo.Data.getUsername());
            // get username to be used for other scene since gameobject gets destroyed when changing scene
        }
    }

    public void ToSelect() //Changes screen to SelectLevels.
    {
        //Disable this screen enable other screen.
        Debug.Log("[Play] button clicketty click! Accessing MainMenu.cs");
        MenuScreen.SetActive(false);
        SelectScreen.SetActive(true);
        

    }

    public void ToLeaderboards() //Changes screen to leaderboard.
    {
        MenuScreen.SetActive(false);
        LeaderboardsScreen.SetActive(true);
    }

    public void ToDashboard()//Changes screen to dashboard.
    {
        MenuScreen.SetActive(false);
        DashboardScreen.SetActive(true);
    }

    public void ToChallenges()//Changes screen to challenges.
    {
        MenuScreen.SetActive(false);
        ChallengesScreen.SetActive(true);
    }

    public void ToLogOut() //Logs user out to login screen.
    {
        LoginMenu.LogOut();
        MenuScreen.SetActive(false);
        LoginScreen.SetActive(true);
    }
    public void QuitGame() //deprecated, not in use.
    {
        Debug.Log("[Quit] button clicketty click! Accessing MainMenu.cs");
        Application.Quit();
    }

    public void ToHelp() //Displays help screen.
    {
        HelpScreen.SetActive(!helpToggle);
        helpToggle = !helpToggle;
    }
}

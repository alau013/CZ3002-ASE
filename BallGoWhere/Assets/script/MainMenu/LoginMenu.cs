using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LoginMenu : MonoBehaviour
{
    public GameObject LoginScreen;
    public GameObject MainScreen;
    public TMP_InputField username;
    public TMP_Text display; //from mainmenu
    public static Boolean loginSuccess = false;
    public static string playerName = null;
    private void Awake()
    {
        if (loginSuccess)
        {
            LoginScreen.SetActive(false);
            MainScreen.SetActive(true);
        }
    }
    public void Login()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        string nameStr = username.text;

        Debug.Log("[Login] button clicketty click! Accessing LoginMenu.cs");
        
        Debug.Log(nameStr +" on device: "+ deviceID);

        //Insert login authentication stuff

        //After successful login, set display msg in MainScreen
        LoginMenu.loginSuccess = true;
        if (loginSuccess)
        {
            LoginMenu.playerName = nameStr;
            Debug.Log(playerName + "logged in");
            string welcomeMsg = "Welcome " + playerName+"!";
            display.text = welcomeMsg;
            LoginScreen.SetActive(false);
            MainScreen.SetActive(true);
            
        }
    }

    public static void LogOut()
    {
        LoginMenu.loginSuccess = false;
        LoginMenu.playerName = null;
    }

   
}

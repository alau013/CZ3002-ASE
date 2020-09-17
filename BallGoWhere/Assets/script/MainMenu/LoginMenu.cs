﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginMenu : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_Text display; //from mainmenu
    public void Login()
    {
        Debug.Log("[Login] button clicketty click! Accessing LoginMenu.cs");
        Debug.Log(username.text);

        //Insert login authentication stuff

        //After successful login, display welcome msg in menu screen.
        string welcomeMsg = "Welcome! " + username.text;
        display.text = welcomeMsg;
    }
}
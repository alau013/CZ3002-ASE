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
    public TMP_Text LoginWarningText;
    public TMP_Text display; //from mainmenu
    public static Boolean loginSuccess = false;
    public static string playerName = null;

    private Transform scrollView;
    private Transform viewPort;

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<LoginEntry> loginEntryList;
    private List<Transform> loginEntryTransformList;

    private void Awake()
    {
        if (loginSuccess)
        {
            LoginScreen.SetActive(false);
            MainScreen.SetActive(true);
        }

        else
        {
            this.LoginWarningText.text = "";
            scrollView = transform.Find("Scroll View");
            viewPort = scrollView.Find("Viewport");
            entryContainer = viewPort.Find("Content");
            entryTemplate = entryContainer.Find("EntryTemplate");

            entryTemplate.gameObject.SetActive(false);

            loginEntryList = new List<LoginEntry>()
        {
            new LoginEntry{name = "ahHuat10"},
            new LoginEntry{name = "bond179"},
            new LoginEntry{name = "doraemon"},
        };

            loginEntryTransformList = new List<Transform>();
            foreach (LoginEntry loginEntry in loginEntryList)
            {
                CreateLoginEntryTransform(loginEntry, entryContainer, loginEntryTransformList);

            }
        }
        
    }

    public void SetName(TMP_Text tmpName)
    {
        this.username.text = tmpName.text;
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

            //save successful username to playerPrefs

        }

        else
        {
            this.LoginWarningText.text = "Username already taken! Please try another...";
        }
    }

    public static void LogOut()
    {
        LoginMenu.loginSuccess = false;
        LoginMenu.playerName = null;
    }

    //Represents a single leaderboard entry
    private void CreateLoginEntryTransform(LoginEntry loginEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 60f;
        //generate objects for each row
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        string name = loginEntry.name;

        //set name for each row.
        entryTransform.Find("nameButton").GetComponentInChildren<TMP_Text>().text = name;

        transformList.Add(entryTransform);
    }
    private class LoginEntry
    {
        public string name;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LoginMenu : MonoBehaviour
{
    public GameObject APIObject;
    public GameObject PrefObject;
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
    private PlayerPrefUI playerInfo;
    private List<String> usernamesList;

    private void OnEnable() 
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

            /*
            loginEntryList = new List<LoginEntry>()
        {
            new LoginEntry{name = "ahHuat10"},
            new LoginEntry{name = "bond179"},
            new LoginEntry{name = "doraemon"},
        };
        */
            loginEntryList = new List<LoginEntry>();
            playerInfo = PrefObject.GetComponent<PlayerPrefUI>();
            usernamesList = playerInfo.availablePlayers();
            if (usernamesList.Count > 0)
            {
                Debug.Log("usernamesList Count: " + usernamesList.Count.ToString());
                foreach (string username in usernamesList)
                {
                    loginEntryList.Add(new LoginEntry { name = username });
                }
            }
            
            //test above
            loginEntryTransformList = new List<Transform>();
            if (loginEntryList.Count > 0)
            {
                Debug.Log("loginEntryList old Count: " + loginEntryList.Count);
                ResetContent(entryContainer);
                foreach (LoginEntry loginEntry in loginEntryList)
                {
                    CreateLoginEntryTransform(loginEntry, entryContainer, loginEntryTransformList);

                }
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

        //Test for accessing api endpoint - very important chuck norris api in this example :)
        APIScript AccessAPI = APIObject.GetComponent<APIScript>();
        AccessAPI.GetChuckling();
        //test above

        if (nameStr != "")
        {
            //Insert login authentication stuff
            //After successful login, set display msg in MainScreen
            LoginMenu.loginSuccess = true;
            if (loginSuccess)
            {
                //PlayerPrefs.DeleteAll(); //use this to reset PlayerPrefs when testing
                playerInfo = PrefObject.GetComponent<PlayerPrefUI>();
                usernamesList = playerInfo.availablePlayers();

                if (usernamesList.Count > 0)
                {
                    if (usernamesList.Contains(nameStr)) //load old username
                    {
                        playerInfo.Load(nameStr);
                    }
                    else //save new username
                    {
                        playerInfo.setUsername(nameStr);
                        playerInfo.Save();
                    }
                }
                else //save new username
                {
                    playerInfo.setUsername(nameStr);
                    playerInfo.Save();
                }


                LoginMenu.playerName = nameStr;
                Debug.Log(playerName + "logged in");
                string welcomeMsg = "Welcome " + playerName + "!";
                display.text = welcomeMsg;
                LoginScreen.SetActive(false);
                MainScreen.SetActive(true);


            }

            else
            {
                this.LoginWarningText.text = "Username already taken! Please try another...";
            }
        }
        else
        {
            this.LoginWarningText.text = "Please Enter A Username!";
        }
        
    }

    public static void LogOut()
    {
        LoginMenu.loginSuccess = false;
        LoginMenu.playerName = null;
    }

    private void ResetContent(Transform entryContainer)
    {
        var children = new List<GameObject>();
        foreach (Transform child in entryContainer) {
            if(child.name != "EntryTemplate")
            {
                children.Add(child.gameObject);
            }
            

         }
        children.ForEach(child => Destroy(child));


    }
    //Represents a single leaderboard entry
    private void CreateLoginEntryTransform(LoginEntry loginEntry, Transform container, List<Transform> transformList)
    {
        Debug.Log("[CreateLoginEntryTransform] transformList count: " + transformList.Count.ToString());
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

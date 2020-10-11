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
    private LoginResponseAPI loginData = new LoginResponseAPI();

    private void OnEnable() 
    {
        playerInfo = PrefObject.GetComponent<PlayerPrefUI>();
        if (loginSuccess)
        {
            LoginScreen.SetActive(false);
            MainScreen.SetActive(true);
        }

        else
        {
            this.LoginWarningText.text = "";
            if (transform.Find("Scroll View")!=null)
            {scrollView = transform.Find("Scroll View");
                if (scrollView.Find("Viewport") != null)
                {viewPort = scrollView.Find("Viewport");
                    if (viewPort.Find("Content")!=null)
                    {entryContainer = viewPort.Find("Content");
                        if (entryContainer.Find("EntryTemplate") != null)
                        {entryTemplate = entryContainer.Find("EntryTemplate");
                         entryTemplate.gameObject.SetActive(false);
                        }
                    }
                }
            }
            

            /*
            loginEntryList = new List<LoginEntry>()
        {
            new LoginEntry{name = "ahHuat10"},
            new LoginEntry{name = "bond179"},
            new LoginEntry{name = "doraemon"},
        };
        */
            loginEntryList = new List<LoginEntry>();
            
            usernamesList = playerInfo.getNameList();
            //usernamesList = playerInfo.availablePlayers();
            
            if (usernamesList.Count > 0)
            {
                foreach (string username in usernamesList)
                {
                    loginEntryList.Add(new LoginEntry { name = username });
                }
            }
            
            //test above
            loginEntryTransformList = new List<Transform>();
            if (loginEntryList.Count > 0)
            {
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

        APIScript AccessAPI = APIObject.GetComponent<APIScript>();

        
        
        if (nameStr != "")
        {
            ArrayList arr = AccessAPI.PostLogin(nameStr);
            if (!arr[0].Equals("ERROR"))
            {
                //PlayerPrefs.DeleteAll(); //use this to reset PlayerPrefs when testing
                bool validLogin = true;
                if (!validLogin) //modify to check if login is valid by reading the result json string.
                {
                    this.LoginWarningText.text = "Invalid Username! Please try another...";
                }
                else
                {
                    LoginMenu.loginSuccess = true;
                    loginData = (LoginResponseAPI)arr[1];
                    usernamesList = playerInfo.getNameList();
                    playerInfo.Data.setUsername(nameStr);
                    playerInfo.SaveDataToPlayerPref();

                    if (usernamesList.Count > 0)
                    {
                        if (usernamesList.Contains(nameStr)) //load old username and username-linked playerpref data
                        {
                            playerInfo.LoadDataFromPlayerPref(nameStr);

                        }
                        else //save new username
                        {
                            playerInfo.Data.setUsername(nameStr);
                        }
                    }
                    else //save new username
                    {
                        playerInfo.Data.setUsername(nameStr);

                    }

                    playerInfo.Data.LoadLoginData(loginData);
                    playerInfo.Data.updateLastActive();
                    playerInfo.SaveDataToPlayerPref();
                    Debug.Log("[playerInfo.Data]: " + playerInfo.Data.ExportToJson());
                    LoginMenu.playerName = nameStr;
                    Debug.Log(playerName + "logged in");
                    //string welcomeMsg = "Welcome " + playerName + "!";
                    //display.text = welcomeMsg;
                    LoginScreen.SetActive(false);
                    MainScreen.SetActive(true);
                }
                
            }

            else
            {
                
                this.LoginWarningText.text = "Connection Error! Please try again!";
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
        float templateHeight = 60f;
        //generate objects for each row
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        string name = loginEntry.name;

        //set name for each row.
        if (entryTransform.Find("nameButton") != null)
        {
            entryTransform.Find("nameButton").GetComponentInChildren<TMP_Text>().text = name;
            transformList.Add(entryTransform);
        }
        
    }
    private class LoginEntry
    {
        public string name;
    }

}

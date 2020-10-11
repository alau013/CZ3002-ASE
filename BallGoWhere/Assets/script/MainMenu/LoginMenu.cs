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
        //PlayerPrefs.DeleteAll();
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
            if (!arr[0].Equals("ERROR") && !arr[0].Equals("INVALID"))
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

                    //TEST LEADERBOARD POST test
                    //toSubmitLeaderboard("special", 16, 4, "2020-10-07 10:30", 1

                    //TEST Start Challenge PUT test
                    /*
                    String cid = "5f82f9bbd563394597f952ea";
                    ArrayList arrStart = AccessAPI.PutStartChallenge(playerInfo.Data.username, cid);
                    if (arrStart.Count > 1)
                    {
                        Debug.Log("Successfully acquired challenge!");
                    }
                    else
                    {
                        if (arrStart[0].Equals("INVALID"))
                        {
                            Debug.Log("Challenge declined! Someone beat you to it...please try another!");
                        }
                        else
                        {
                            Debug.Log("Error processing challenge!");
                        }
                    }
                    */
                    //String testStr = String.Format("/challenge/start/{0}", cid);
                    
                    //AccessAPI.Put(testStr, new StartChallengeEntryAPI(playerInfo.Data.username));
                    
                    
                    LoginScreen.SetActive(false);
                    MainScreen.SetActive(true);
                }
                
            }

            else
            {
                if (arr[0].Equals("INVALID"))
                {
                    this.LoginWarningText.text = "Username is already taken! Please try another.";
                }
                else
                {
                    this.LoginWarningText.text = "Connection Error! Please try again!";
                }
                
            }
        }
        else
        {
            this.LoginWarningText.text = "Please Enter A Username!";
        }
        
    }

    public void toSubmitLeaderboard(string leaderboardType, int score, int time, string date_time, int level)
    {
        //test leaderboard post method.
        APIScript AccessAPI = APIObject.GetComponent<APIScript>();
        int currHighscore = 0;
        currHighscore = playerInfo.Data.getLeaderboardScore(leaderboardType, level);
        if (score > currHighscore)
        {
            Debug.Log("Current highscore is " + currHighscore + ". Submitting new highscore: " + score);
            ArrayList arr = AccessAPI.PostLeaderboard(playerInfo.Data.username, leaderboardType, new StandardEntryAPI(score, time, date_time, level));
            if (!arr[0].Equals("ERROR"))
            {
                LoginResponseAPI leaderboardData = (LoginResponseAPI)arr[1];
                playerInfo.Data.LoadLoginData(leaderboardData);
                playerInfo.SaveDataToPlayerPref();
            }
            else
            {
                Debug.Log("Connection error. Failed to update new highscore...");
            }
        }
        else
        {
            Debug.Log("Current highscore of " + currHighscore + " is higher than " + score+". Score not submitted.");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class ChallengesScript : MonoBehaviour
{
    public GameObject PrefObject;
    public GameObject APIObject;
    public GameObject ChallengesScreen;
    public GameObject HelpScreen;
    public GameObject ChallengesError;
    private bool helpToggle = false;
    private Transform scrollView;
    private Transform viewPort;

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<ChallengeEntry> challengeEntryList;
    private List<Transform> challengeEntryTransformList;
    PlayerPrefUI playerInfo;

    private void OnEnable()
    {
        playerInfo = PrefObject.GetComponent<PlayerPrefUI>();
        if (transform.Find("Scroll View") != null)
        {
            scrollView = transform.Find("Scroll View");
            if (scrollView.Find("Viewport") != null)
            {
                viewPort = scrollView.Find("Viewport");
                if (viewPort.Find("Content") != null)
                {
                    entryContainer = viewPort.Find("Content");
                    if (entryContainer.Find("ChallengeTemplate") != null)
                    {
                        entryTemplate = entryContainer.Find("ChallengeTemplate");
                        entryTemplate.gameObject.SetActive(false);
                    }
                }
            }
        }


        entryTemplate.gameObject.SetActive(false);
        //modify to load challenges from api..
        ArrayList results = GetChallenges();
        Debug.Log("[ChallengesScript.cs OnEnable()]: ");
        Debug.Log(results);

        challengeEntryList = new List<ChallengeEntry>()
        {
            new ChallengeEntry{state = "WIN", oppName = "ahHuat10", oppTiming="00:30",timing="00:25",level=3},
            new ChallengeEntry{state = "WIN", oppName = "player179", oppTiming="01:10",timing="00:50",level=2},
            new ChallengeEntry{state = "LOSS", oppName = "doraemon", oppTiming="01:00",timing="02:30",level=4},
            new ChallengeEntry{state = "START", oppName = "doraemon", oppTiming="?",timing="?",level=1},
            new ChallengeEntry{state = "START", oppName = "randomPlay1", oppTiming="?",timing="?",level=1},
        };

        challengeEntryTransformList = new List<Transform>();
        ResetContent(entryContainer);
        foreach (ChallengeEntry challengeEntry in challengeEntryList)
        {
            CreateChallengeEntryTransform(challengeEntry, entryContainer, challengeEntryTransformList);

        }
    }
    private void Awake()
    {
        
    }

    private void ResetContent(Transform entryContainer)
    {
        var children = new List<GameObject>();
        foreach (Transform child in entryContainer)
        {
            if (child.name != "ChallengeTemplate")
            {
                children.Add(child.gameObject);
            }


        }
        children.ForEach(child => Destroy(child));


    }
    private void CreateChallengeEntryTransform(ChallengeEntry challengeEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 60f;
        //generate objects for each row
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        string state = challengeEntry.state;
        string oppName = challengeEntry.oppName;
        string oppTiming = challengeEntry.oppTiming;
        string timing = challengeEntry.timing;
        int level = challengeEntry.level;

        if (entryTransform.Find("stateButton") != null && entryTransform.Find("oppNameText") != null && entryTransform.Find("challengeLevelText") != null && entryTransform.Find("oppTimingText")!=null && entryTransform.Find("timingText")!=null)
        {
            //set pos,name and score for each row.

            entryTransform.Find("stateButton").GetComponentInChildren<TMP_Text>().text = state;
            entryTransform.Find("oppNameText").GetComponent<TMP_Text>().text = oppName;
            entryTransform.Find("challengeLevelText").GetComponent<TMP_Text>().text = level.ToString();


            if (state == "WIN" || state == "LOSS")
            {
                UnityEngine.UI.Button stateButton = entryTransform.Find("stateButton").GetComponent<UnityEngine.UI.Button>();
                stateButton.interactable = false;
                entryTransform.Find("oppTimingText").GetComponent<TMP_Text>().text = oppTiming;
                entryTransform.Find("timingText").GetComponent<TMP_Text>().text = timing;
            }
            else
            {
                entryTransform.Find("oppTimingText").GetComponent<TMP_Text>().text ="?";
                entryTransform.Find("timingText").GetComponent<TMP_Text>().text ="?";
                UnityEngine.UI.Button stateButton = entryTransform.Find("stateButton").GetComponent<UnityEngine.UI.Button>();
                stateButton.onClick.AddListener(delegate { OnStartChallenge(level); });

            }

            transformList.Add(entryTransform);
        }
    }

    public void OnStartChallenge(int challengeLevel)
    {
        Debug.Log("Challenge started!");
        ChallengesScreen.SetActive(false);
        IDictionary<int, string> scenesDict = new Dictionary<int, string>();
        scenesDict.Add(1, "level 1");
        scenesDict.Add(6, "special level");

        SceneManager.LoadScene(scenesDict[challengeLevel]);
    }

    private class ChallengeEntry
    {
        public string state; //"WIN", "LOSS", "START"
        public string oppName;
        public string oppTiming;
        public string timing;
        public int level;
        

    }

    [Serializable]
    public class ChallengesAPI
    {
        public string state;
        public string recvTime;
        public string _id;
        public string date_time;
        public string senderName;
        public int senderTime;
        public int level;
    }

    public ArrayList GetChallenges() 
    {
        APIScript AccessAPI = APIObject.GetComponent<APIScript>();

        ArrayList resultsList = new ArrayList();
        string apiLink = String.Format("/challenge/{0}", playerInfo.Data.username);
        
        string jsonResponse = AccessAPI.GetResponse(apiLink); //modify this if you move this to APIScript.cs
        if (jsonResponse.Equals("ERROR"))
        {
            resultsList.Add(false);
        }
        else
        {
            resultsList.Add(true);
            Debug.Log("RECEIVED: " + jsonResponse);
            //ChallengesAPI info = JsonUtility.FromJson<ChallengesAPI>(jsonResponse);
            //resultsList.Add(info);
        }

        //test leaderboard post method.
        /*
        ArrayList arr =  AccessAPI.PostLeaderboard(playerInfo.Data.username, "special", new StandardEntryAPI(10, 5, "2020-10-07 08:30", 1));
        if (!arr[0].Equals("ERROR"))
        {
            LeaderboardResponseAPI leaderboardResponse = (LeaderboardResponseAPI)arr[1];
            LoginResponseAPI loginData = leaderboardResponse.info;
            playerInfo.Data.LoadLoginData(loginData);
            playerInfo.SaveDataToPlayerPref();
            Debug.Log("special count: "+playerInfo.Data.special.Count);

            Debug.Log("[LeaderboardResponse TEST]: "+playerInfo.Data.getLeaderboardScore("special",4));
        }
        */
        
        return resultsList;
    }

    public void ToHelp()
    {
        HelpScreen.SetActive(!helpToggle);
        helpToggle = !helpToggle;
    }
}

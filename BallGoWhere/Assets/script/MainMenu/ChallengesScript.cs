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
    public GameObject ChallengesInvalidPanel;
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
    private bool flagInvalidChallenge = false;

    private void OnEnable()
    {
        playerInfo = PrefObject.GetComponent<PlayerPrefUI>();
        if (flagInvalidChallenge)
        {
            ChallengesInvalidPanel.SetActive(true);
            flagInvalidChallenge = false;
        }
        else
        {
            ChallengesInvalidPanel.SetActive(false);
        }
        ChallengesError.SetActive(false);
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
        APIScript AccessAPI = APIObject.GetComponent<APIScript>();
        ArrayList results = AccessAPI.GetChallenges();
        //Debug.Log("[ChallengesScript.cs OnEnable()]: "
        challengeEntryList = new List<ChallengeEntry>();
        challengeEntryTransformList = new List<Transform>();

        if (results.Count > 1 && results[0].Equals(true))
        {
            ResetContent(entryContainer);
            ChallengesAPI info = (ChallengesAPI)results[1];
            foreach (ChallengeEntryAPI item in info.challenges)
            {
                challengeEntryList.Add(new ChallengeEntry(item.state, item.senderName, item.senderTime, item.recvTime, item.level, item.type, item._id));
            }

            foreach (ChallengeEntry challengeEntry in challengeEntryList)
            {
                CreateChallengeEntryTransform(challengeEntry, entryContainer, challengeEntryTransformList);

            }
        }
        else
        {
            ChallengesError.SetActive(true);
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
        string oppTiming = challengeEntry.oppTiming.ToString();
        string timing = challengeEntry.timing.ToString();
        int level = challengeEntry.level;
        string challengeId = challengeEntry._id;

        if (entryTransform.Find("stateButton") != null && entryTransform.Find("oppNameText") != null && entryTransform.Find("challengeLevelText") != null && entryTransform.Find("oppTimingText") != null && entryTransform.Find("timingText") != null)
        {
            //set pos,name and score for each row.

            entryTransform.Find("stateButton").GetComponentInChildren<TMP_Text>().text = state;
            entryTransform.Find("oppNameText").GetComponent<TMP_Text>().text = oppName;
            entryTransform.Find("challengeLevelText").GetComponent<TMP_Text>().text = level.ToString();
            entryTransform.Find("challengeIdText").GetComponent<TMP_Text>().text = challengeId;


            if (state == "win" || state == "loss")
            {
                UnityEngine.UI.Button stateButton = entryTransform.Find("stateButton").GetComponent<UnityEngine.UI.Button>();
                stateButton.interactable = false;
                entryTransform.Find("oppTimingText").GetComponent<TMP_Text>().text = oppTiming;
                entryTransform.Find("timingText").GetComponent<TMP_Text>().text = timing;
            }
            else
            {
                entryTransform.Find("oppTimingText").GetComponent<TMP_Text>().text = "?";
                entryTransform.Find("timingText").GetComponent<TMP_Text>().text = "?";
                UnityEngine.UI.Button stateButton = entryTransform.Find("stateButton").GetComponent<UnityEngine.UI.Button>();
                stateButton.onClick.AddListener(delegate { OnStartChallenge(level, challengeId, challengeEntry.oppTiming); });

            }

            transformList.Add(entryTransform);
        }
    }

    public string CheckStartChallenge(string challengeId)
    {
        string result = "";
        //challengeId = "5f82f9bbd563394597f952ea";
        APIScript AccessAPI = APIObject.GetComponent<APIScript>();
        ArrayList arrStart = AccessAPI.PutStartChallenge(playerInfo.Data.username, challengeId);
        result = arrStart[0].ToString();

        return result;
    }
    public void OnStartChallenge(int challengeLevel, string challengeId, int oppTiming)
    {

        IDictionary<int, string> scenesDict = new Dictionary<int, string>();
        scenesDict.Add(1, "level 1");
        scenesDict.Add(2, "level 2");
        scenesDict.Add(3, "level 3");
        //scenesDict.Add(6, "special level");
        string result = CheckStartChallenge(challengeId);
        if (result.Equals("INVALID"))
        {
            ChallengesInvalidPanel.SetActive(true);
            flagInvalidChallenge = true;
            Debug.Log("Challenge declined! Someone beat you to it...please try another!");
            OnEnable(); //reloads screen
        }
        else if (result.Equals("ERROR"))
        {
            ChallengesInvalidPanel.SetActive(false);
            ChallengesError.SetActive(true);
            Debug.Log("Connection Error!"); //internet connection error
        }
        else
        {
            Debug.Log("Challenge accepted!");
            ChallengesInvalidPanel.SetActive(false);
            ChallengesError.SetActive(false);
            ChallengesScreen.SetActive(false);
            if (scenesDict.ContainsKey(challengeLevel))
            {
                //pass [true/false, challengeId, oppTiming] to challengeHolder variable
                playerInfo.LoadDataFromPlayerPref(LoginMenu.playerName);
                PlayerPrefs.SetInt("cc",1);
                playerInfo.Data.challengeHolder = new ArrayList();
                playerInfo.Data.challengeHolder.Add(true);
                playerInfo.Data.challengeHolder.Add(challengeId);
                playerInfo.Data.challengeHolder.Add(oppTiming);
                PlayerPrefs.SetString("cid",challengeId);
                PlayerPrefs.SetInt("oppotime",oppTiming);
                playerInfo.SaveDataToPlayerPref();
                SceneManager.LoadScene(scenesDict[challengeLevel]);
            }
        }


    }

    private class ChallengeEntry
    {
        public string state; //"WIN", "LOSS", "START"
        public string oppName;
        public int oppTiming;
        public int timing;
        public int level;
        public string type;
        public string _id;

        public ChallengeEntry(string state, string oppName, int oppTiming, int timing, int level, string type, string _id)
        {
            this.state = state;
            this.oppName = oppName;
            this.oppTiming = oppTiming;
            this.timing = timing;
            this.level = level;
            this.type = type;
            this._id = _id;
        }


    }


    public void ToHelp()
    {
        HelpScreen.SetActive(!helpToggle);
        helpToggle = !helpToggle;
    }
}

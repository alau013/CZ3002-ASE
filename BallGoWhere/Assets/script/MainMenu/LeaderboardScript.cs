using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
public class LeaderboardScript : MonoBehaviour
{
    private Transform scrollView;
    private Transform viewPort;
    public GameObject PrefObject;
    public GameObject APIObject;
    public GameObject LeaderboardError;
    public Button TypeButton;
    public GameObject HelpScreen;
    private bool helpToggle = false;

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<LeaderboardEntry> leaderboardEntryList;
    private List<Transform> leaderboardEntryTransformList;
    private int vizMode = 0; //0 - Weekly, 1- Special, 2- All-time
    private int numModes = 3;
    private void Awake()
    {
    }

    private void OnEnable()
    {
        if (transform.Find("Scroll View") != null)
        {
            scrollView = transform.Find("Scroll View");
            if (scrollView.Find("Viewport") != null)
            {
                viewPort = scrollView.Find("Viewport");
                if (viewPort.Find("Content") != null)
                {
                    entryContainer = viewPort.Find("Content");
                    if (entryContainer.Find("EntryTemplate") != null)
                    {
                        entryTemplate = entryContainer.Find("EntryTemplate");
                        entryTemplate.gameObject.SetActive(false);
                    }
                }
            }
        }


        leaderboardEntryList = new List<LeaderboardEntry>();
        leaderboardEntryTransformList = new List<Transform>();
        APIScript AccessAPI = APIObject.GetComponent<APIScript>();
        ArrayList results = new ArrayList();
        if (vizMode == 0)
        {//Weekly
            results = AccessAPI.GetLeaderboard("weekly");
            TypeButton.GetComponentInChildren<TMP_Text>().text = "Weekly";
        }
        else if (vizMode == 1)
        {//Special
            results = AccessAPI.GetLeaderboard("special");
            TypeButton.GetComponentInChildren<TMP_Text>().text = "Special";
        }
        else
        {//All-Time
            results = AccessAPI.GetLeaderboard("all_time");
            TypeButton.GetComponentInChildren<TMP_Text>().text = "All-Time";
        }
        
        ResetContent(entryContainer);

        if (results[0].Equals(true) && results.Count>1)
        {
            LeaderboardError.SetActive(false);
            LeaderboardAPI info = (LeaderboardAPI)results[1];
            foreach (BoardEntryAPI item in info.board)
            {
                leaderboardEntryList.Add(new LeaderboardEntry { score = item.score, name = item.name });
            }

            if (leaderboardEntryList.Count > 0)
            {
                
                foreach (LeaderboardEntry leaderboardEntry in leaderboardEntryList)
                {
                    CreateLeaderboardEntryTransform(leaderboardEntry, entryContainer, leaderboardEntryTransformList);

                }
            }
        }
        else
        {
            Debug.Log("[LeaderboardScript.cs OnEnable()]: Error accessing API!");
            LeaderboardError.SetActive(true);
        }
        
        
        
    }

    public void ToHelp()
    {
        HelpScreen.SetActive(!helpToggle);
        helpToggle = !helpToggle;
    }

    public void togglePrevMode()
    {
        vizMode += 1;
        if (vizMode >= numModes)
        {
            vizMode = 0;
        }
        
        OnEnable();
    }

    public void toggleNextMode()
    {
        vizMode -= 1;
        if (vizMode < 0)
        {
            vizMode = 2;
        }
        
        OnEnable();
    }
    private void ResetContent(Transform entryContainer)
    {
        var children = new List<GameObject>();
        foreach (Transform child in entryContainer)
        {
            if (child.name != "EntryTemplate")
            {
                children.Add(child.gameObject);
            }


        }
        children.ForEach(child => Destroy(child));


    }
    //Represents a single leaderboard entry

    private void CreateLeaderboardEntryTransform(LeaderboardEntry leaderboardEntry, Transform container, List<Transform> transformList)
    {
        

        float templateHeight = 60f;
        //generate objects for each row
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string name = leaderboardEntry.name;
        int score = leaderboardEntry.score;
        if(entryTransform.Find("posText")!=null && entryTransform.Find("nameText")!=null && entryTransform.Find("scoreText") != null)
        {
            //set pos,name and score for each row.
            entryTransform.Find("posText").GetComponent<TMP_Text>().text = rank.ToString();
            entryTransform.Find("nameText").GetComponent<TMP_Text>().text = name;
            entryTransform.Find("scoreText").GetComponent<TMP_Text>().text = score.ToString();

            transformList.Add(entryTransform);
        }
        
    }
    private class LeaderboardEntry
    {
        public int score;
        public string name;
    }
}

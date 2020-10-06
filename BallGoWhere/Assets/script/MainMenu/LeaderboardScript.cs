using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardScript : MonoBehaviour
{
    private Transform scrollView;
    private Transform viewPort;
    public GameObject APIObject;

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<LeaderboardEntry> leaderboardEntryList;
    private List<Transform> leaderboardEntryTransformList;

    private void Awake()
    {/*
        scrollView = transform.Find("Scroll View");
        viewPort = scrollView.Find("Viewport");
        entryContainer = viewPort.Find("Content");
        entryTemplate = entryContainer.Find("EntryTemplate");

       //entryContainer = transform.Find("Content");
        //entryTemplate = entryContainer.Find("EntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        leaderboardEntryList = new List<LeaderboardEntry>()
        {
            new LeaderboardEntry{score = 120, name = "ahHuat10"},
            new LeaderboardEntry{score = 100, name = "player179"},
            new LeaderboardEntry{score = 98, name = "phoonHuat"},
            new LeaderboardEntry{score = 76, name = "jaslyn97"},
            new LeaderboardEntry{score = 67, name = "anotherPlayer2"},
            new LeaderboardEntry{score = 31, name = "bondJames"},
            new LeaderboardEntry{score = 21, name = "randomPlay1"},
            new LeaderboardEntry{score = 20, name = "platypus"},
            new LeaderboardEntry{score = 15, name = "doraemon"},
        };

        leaderboardEntryTransformList = new List<Transform>();
        foreach(LeaderboardEntry leaderboardEntry in leaderboardEntryList)
        {
            CreateLeaderboardEntryTransform(leaderboardEntry, entryContainer, leaderboardEntryTransformList);

        }
        */
    }

    private void OnEnable()
    {
        scrollView = transform.Find("Scroll View");
        viewPort = scrollView.Find("Viewport");
        entryContainer = viewPort.Find("Content");
        entryTemplate = entryContainer.Find("EntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        APIScript AccessAPI = APIObject.GetComponent<APIScript>();
        LeaderboardAPI info = AccessAPI.GetBoard();
        leaderboardEntryList = new List<LeaderboardEntry>();
        leaderboardEntryTransformList = new List<Transform>();

        foreach (BoardEntryAPI item in info.board)
        {
            leaderboardEntryList.Add(new LeaderboardEntry { score = item.score, name = item.name });
        }
        if (leaderboardEntryList.Count > 0)
        {
            ResetContent(entryContainer);
            foreach (LeaderboardEntry leaderboardEntry in leaderboardEntryList)
            {
                CreateLeaderboardEntryTransform(leaderboardEntry, entryContainer, leaderboardEntryTransformList);

            }
        }
        
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

        //set pos,name and score for each row.
        entryTransform.Find("posText").GetComponent<TMP_Text>().text = rank.ToString();
        entryTransform.Find("nameText").GetComponent<TMP_Text>().text = name;
        entryTransform.Find("scoreText").GetComponent<TMP_Text>().text = score.ToString();

        transformList.Add(entryTransform);
    }
    private class LeaderboardEntry
    {
        public int score;
        public string name;
    }
}

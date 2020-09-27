using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengesScript : MonoBehaviour
{
    private Transform scrollView;
    private Transform viewPort;

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<ChallengeEntry> challengeEntryList;
    private List<Transform> challengeEntryTransformList;
    private void Awake()
    {
        scrollView = transform.Find("Scroll View");
        viewPort = scrollView.Find("Viewport");
        entryContainer = viewPort.Find("Content");
        entryTemplate = entryContainer.Find("ChallengeTemplate");

        
        entryTemplate.gameObject.SetActive(false);

        challengeEntryList = new List<ChallengeEntry>()
        {
            new ChallengeEntry{state = "WIN", oppName = "ahHuat10", oppTiming="00:30",timing="00:25",level=3},
            new ChallengeEntry{state = "WIN", oppName = "player179", oppTiming="01:10",timing="00:50",level=2},
            new ChallengeEntry{state = "LOSS", oppName = "doraemon", oppTiming="01:00",timing="02:30",level=4},
        };

        challengeEntryTransformList = new List<Transform>();
        foreach (ChallengeEntry challengeEntry in challengeEntryList)
        {
            CreateChallengeEntryTransform(challengeEntry, entryContainer, challengeEntryTransformList);

        }
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


        //set pos,name and score for each row.

        entryTransform.Find("challengeStateText").GetComponent<TMP_Text>().text = state;
        entryTransform.Find("oppNameText").GetComponent<TMP_Text>().text = oppName;
        entryTransform.Find("oppTimingText").GetComponent<TMP_Text>().text = oppTiming;
        entryTransform.Find("timingText").GetComponent<TMP_Text>().text = timing;
        entryTransform.Find("challengeLevelText").GetComponent<TMP_Text>().text = level.ToString();

        transformList.Add(entryTransform);
    }


    private class ChallengeEntry
    {
        public string state; //"WIN", "LOSS", "PENDING"
        public string oppName;
        public string oppTiming;
        public string timing;
        public int level;
        

    }
}

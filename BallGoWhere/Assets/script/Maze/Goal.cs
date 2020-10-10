using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject WinPanel;
    public GameObject APIObject;
    public GameObject PrefObject;
    public timeController TimeController;

    private PlayerPrefUI playerinfo;
    public int score = 0;
    public int highScore = 0;
    string highScoreKey = "HighScore";

    private void Awake() {
        playerinfo = GameObject.Find("PrefObject").GetComponent<PlayerPrefUI>();
    }
    private void start()
    {
        highScore = PlayerPrefs.GetInt(highScoreKey,0); 
    }
        

     private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
        Color randomlySelectedColor = GetRandomColorWithAlpha();
        GetComponent<Renderer>().material.color = randomlySelectedColor;
        
        Time.timeScale = 0f;
        WinPanel.SetActive(true);
        int score = 100- (TimeController.GetPlayTime()*10);
         if(score>highScore){
             PlayerPrefs.SetInt(highScoreKey, score);
             PlayerPrefs.Save();
         }
        WinPanel.transform.Find("scoreText").GetComponent<Text>().text = "your score: "+ (score).ToString();
        
       List<AttemptEntry> gameAttempt = new List<AttemptEntry>();

         //gameAttempt.Add(new AttemptEntry("10-10-20",1,2));


        //playerinfo.Data.setUsername(PlayerPrefs.GetString("user"));

         string day = System.DateTime.Now.ToString("MM/dd/yyyy");
         playerinfo.Data.addAttempt(new AttemptEntry(day,score,1));

         playerinfo.SaveDataToPlayerPref();
         playerinfo.Data.ExportToJson();

         //Debug.Log("from game level:" + playerinfo.Data.ExportToJson());
         Debug.Log(PlayerPrefs.GetInt("highscore"));

        }
    }

    private Color GetRandomColorWithAlpha()
    {
        return new Color(
            r:UnityEngine.Random.Range(0f,1f),
            g:UnityEngine.Random.Range(0f,1f),
            b:UnityEngine.Random.Range(0f,1f));
        
    }
}

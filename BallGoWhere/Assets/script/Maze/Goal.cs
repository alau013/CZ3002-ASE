using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject WinPanel, levelHandle;
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
        int gameLevel = GameObject.Find("levelHandle").GetComponent<levelHandle>().levelInfo;
        string gameType = GameObject.Find("levelHandle").GetComponent<levelHandle>().type;
        
        Color randomlySelectedColor = GetRandomColorWithAlpha();
        GetComponent<Renderer>().material.color = randomlySelectedColor;
        
        Time.timeScale = 0f;
        WinPanel.SetActive(true);
        int score = 600 / (TimeController.GetPlayTime());
         if(score>highScore){
             PlayerPrefs.SetInt(highScoreKey, score);
             PlayerPrefs.Save();
         }
         if(score < 0)
         {
             score = 0;
         }
        WinPanel.transform.Find("scoreText").GetComponent<Text>().text = "your score: "+ (score).ToString();
        
     
         string day = System.DateTime.Now.ToString("MM/dd/yyyy");
         playerinfo.LoadDataFromPlayerPref(PlayerPrefs.GetString("user"));

         playerinfo.Data.addAttempt(new AttemptEntry(day,score,gameLevel));
         playerinfo.Data.addDailyPlay((float)TimeController.GetPlayTime());

         playerinfo.SaveDataToPlayerPref();

         Debug.Log("from game level:" + playerinfo.Data.ExportToJson());
         //Debug.Log("player score: "+score);

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

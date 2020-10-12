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
    private LoginMenu loginMenu;
    public int score = 0;
    public int highScore = 0;
    string highScoreKey = "HighScore";

    private bool highScrSuccess =false;

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
        highScrSuccess = false;
        
        Color randomlySelectedColor = GetRandomColorWithAlpha();
        GetComponent<Renderer>().material.color = randomlySelectedColor;
        
        Time.timeScale = 0f;
        WinPanel.SetActive(true);
        int score = 600 / (TimeController.GetPlayTime());
         playerinfo.LoadDataFromPlayerPref(PlayerPrefs.GetString("user"));

         Debug.Log(playerinfo.Data.challengeHolder.Count);
        
        
        if (PlayerPrefs.HasKey("cc")) // challenge stage
        {
            APIScript AccessAPI = APIObject.GetComponent<APIScript>();
            string challengeID = PlayerPrefs.GetString("cid");
            int  oppTiming = PlayerPrefs.GetInt("oppotime");

            if (TimeController.GetPlayTime() < oppTiming)
            {
                ArrayList arrTest = AccessAPI.PutUpdateChallenge(playerinfo.Data.username,TimeController.GetPlayTime(),challengeID);
                WinPanel.transform.Find("scoreText").GetComponent<Text>().text = "your timing: "+ TimeController.GetPlayTime().ToString() +" opponent timing: "+oppTiming.ToString();
                if(arrTest[0].Equals("ERROR") || arrTest[0].Equals("INVALID"))
                {
                    Debug.Log("PutUpdateChallenge() failed!");
                }
                else
                {
                    Debug.Log("LoginMenu.cs test [PutUpdateChallenge()]: " + arrTest[0]);
                }
            }
            else
            {
                WinPanel.transform.Find("winText").GetComponent<Text>().text = "You Lose!";
                WinPanel.transform.Find("scoreText").GetComponent<Text>().text = "your timing: "+ TimeController.GetPlayTime().ToString() +" opponent timing: "+oppTiming.ToString();
                Debug.Log("challenge failed");
            }

            PlayerPrefs.DeleteKey("cc");
            PlayerPrefs.Save();
            

        }
        else//no challengeholder object = normal gameplay
        { 
            string day = System.DateTime.Now.ToString("MM/dd/yyyy HH:mm");

            playerinfo.Data.addAttempt(new AttemptEntry(day,score,gameLevel));
            playerinfo.Data.addDailyPlay((float)TimeController.GetPlayTime());

            toSubmitLeaderboard(gameType,score,TimeController.GetPlayTime(),day,gameLevel);
            
            if(highScrSuccess==true)
            {
                playerinfo.SaveDataToPlayerPref();
            }

            Debug.Log("from game level:" + playerinfo.Data.ExportToJson());
            //Debug.Log("player score: "+score);
        }
        }
    }

    private Color GetRandomColorWithAlpha()
    {
        return new Color(
            r:UnityEngine.Random.Range(0f,1f),
            g:UnityEngine.Random.Range(0f,1f),
            b:UnityEngine.Random.Range(0f,1f));
        
    }


    public void toSubmitLeaderboard(string leaderboardType, int score, int time, string date_time, int level)
    {
        //test leaderboard post method.
        APIScript AccessAPI = APIObject.GetComponent<APIScript>();
        int currHighscore = 0;
        currHighscore = playerinfo.Data.getLeaderboardScore(leaderboardType, level);
        if (currHighscore <0)
        {
            currHighscore =0;
        }
        WinPanel.transform.Find("scoreText").GetComponent<Text>().text = "your current score: "+ (score).ToString() +"   Personal High score: "+currHighscore.ToString();
        if (score > currHighscore+1)
        {
            Debug.Log("Current highscore is " + currHighscore + ". Submitting new highscore: " + score);
            toCreateChallenge(level);
            ArrayList arr = AccessAPI.PostLeaderboard(playerinfo.Data.username, leaderboardType, new StandardEntryAPI(score, time, date_time, level));
            if (!arr[0].Equals("ERROR"))
            {
                LoginResponseAPI leaderboardData = (LoginResponseAPI)arr[1];
                playerinfo.Data.LoadLoginData(leaderboardData);
                playerinfo.SaveDataToPlayerPref();
                highScrSuccess = true;
                
            }
            else
            {
                Debug.Log("Connection error. Failed to update new highscore...");
            }
        }
        else
        {
            WinPanel.transform.Find("winText").GetComponent<Text>().text = "Awwwww";
            Debug.Log("Current highscore of " + currHighscore + " is higher than " + score+". Score not submitted.");
        }
        
    }

    public void toCreateChallenge(int level)
    {
        APIScript AccessAPI = APIObject.GetComponent<APIScript>();
        string dateCreated = System.DateTime.Now.ToString("yyyy/dd/MM");

          ArrayList arrTest = AccessAPI.PostCreateChallenge(dateCreated,playerinfo.Data.username,"standard",level);
                if(arrTest[0].Equals("ERROR") || arrTest[0].Equals("INVALID"))
                {
                    Debug.Log("PostCreateChallenge() failed!");
                }
                else
                {
                    Debug.Log("PostCreateChallenge() success!");
                }
    }
}

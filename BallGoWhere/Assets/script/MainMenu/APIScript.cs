using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using TMPro;
using System.Globalization;
using UnityEngine.Rendering;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
//using System.Diagnostics;

//Create an empty game object in each Scene, that links to this APIScript.cs. Then, will be able to access the methods here, as well as
//loading of data to DB when app is closed/exited (see OnFocus() method here).
public class APIScript : MonoBehaviour
{
    public int timeOut;//SET THIS ONLY FROM UNITY
    //public string localHostIp; //SET THIS ONLY FROM UNITY!
    public GameObject PrefObject;
    private PlayerPrefUI playerInfo;
    private bool loginFlag = false;
    private bool leadboardFlag = false;
    private string localHostIp = "http://192.168.1.15:3000"; //.15 for Jaslyn, .5 for Alan.
    public void OnEnable()
    {
        playerInfo = PrefObject.GetComponent<PlayerPrefUI>();
        playerInfo.LoadDataFromPlayerPref(playerInfo.Data.username); //load latest data from playerpref..
        
    }
    public ChuckNorris GetChuckling()
    {
        string apiLink = "https://api.chucknorris.io/jokes/random";
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", CityId, API_KEY));
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiLink);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        ChuckNorris info = JsonUtility.FromJson<ChuckNorris>(jsonResponse);
        UnityEngine.Debug.Log("[GetChuckling()]...");
        UnityEngine.Debug.Log(info.value);
        return info;
    }

    
    public string GetResponse(string apiLink)
    {
        apiLink = this.localHostIp + apiLink;
        Debug.Log("this.localHostIp: "+this.localHostIp);
        string jsonResponse = "ERROR";
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiLink);
            HttpWebResponse response;
            var task = Task.Run(async () => await request.GetResponseAsync());
            if (task.Wait(TimeSpan.FromSeconds(this.timeOut)))
            {
                response = (HttpWebResponse)task.Result;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                jsonResponse = reader.ReadToEnd();
            }
            else
            {
                new Exception("Timed out");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("[APIScript.cs GetResponse()]: ERROR! "+e.Message);
            //throw;
        }
        return jsonResponse;
    }

    public string Post2(string apiLink, object dataObject)
    {
        apiLink = this.localHostIp + apiLink;
        string jsonStr = JsonUtility.ToJson(dataObject);
        string jsonResponse = "ERROR";
        string contentStr = "";
        try
        {

            using (var client = new HttpClient(new HttpClientHandler { UseProxy = false }))
            {
                var task = Task.Run(async () =>
                {
                    var response = await client.PostAsync(apiLink, new StringContent(jsonStr, Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    contentStr = content.ToString();
                    Debug.Log("[APIScript.cs Post2() content]: " + contentStr);
                });
                if (task.Wait(TimeSpan.FromSeconds(this.timeOut)))
                {
                    //jsonResponse = "SUCCESS";
                    jsonResponse = contentStr;//replace with this when the api response has a flag for success/username already taken. you will need to modify LoginMenu.cs accordingly as well.
                }
                else
                {
                    new Exception("Timed out");
                }
            }
                
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("[APIScript.cs GetResponse()]: ERROR! " + e.Message);
            //throw;
        }
        return jsonResponse;

        

    }

    public string Put(string apiLink, object dataObject)
    {
        apiLink = this.localHostIp + apiLink;
        string jsonStr = JsonUtility.ToJson(dataObject);
        string jsonResponse = "ERROR";
        string contentStr = "";
        try
        {

            using (var client = new HttpClient(new HttpClientHandler { UseProxy = false }))
            {
                var task = Task.Run(async () =>
                {
                    var response = await client.PutAsync(apiLink, new StringContent(jsonStr, Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    contentStr = content.ToString();
                    Debug.Log("[APIScript.cs Put() content]: " + contentStr);
                });
                if (task.Wait(TimeSpan.FromSeconds(this.timeOut)))
                {
                    //jsonResponse = "SUCCESS";
                    jsonResponse = contentStr;//replace with this when the api response has a flag for success/username already taken. you will need to modify LoginMenu.cs accordingly as well.
                }
                else
                {
                    new Exception("Timed out");
                }
            }

        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("[APIScript.cs GetResponse()]: ERROR! " + e.Message);
            //throw;
        }
        return jsonResponse;



    }

   
    public ArrayList GetLeaderboard()
    {
        ArrayList resultsList = new ArrayList();
        String dateStr = System.DateTime.Now.ToString(@"yyyy-MM-dd");
        string apiLink = String.Format("/score/leaderboard/weekly?date=\"{0}\"", dateStr);
        
        string jsonResponse = GetResponse(apiLink);
        if (jsonResponse.Equals("ERROR"))
        {
            resultsList.Add(false);
        }
        else
        {
            resultsList.Add(true);
            LeaderboardAPI info = JsonUtility.FromJson<LeaderboardAPI>(jsonResponse);
            resultsList.Add(info);
        }
        return resultsList;
    }

    public ArrayList GetLeaderboard(string mode)
    {
        ArrayList resultsList = new ArrayList();
        String dateStr = System.DateTime.Now.ToString(@"yyyy-MM-dd");
        string apiLink = "";

        if (mode == "weekly")
        {
            apiLink = String.Format("/score/leaderboard/weekly?date=\"{0}\"", dateStr);
        }
        else if (mode == "special")
        {
            apiLink = String.Format("/score/leaderboard/special");
        }
        else
        {
            apiLink = String.Format("/score/leaderboard/all_time");
        }

        string jsonResponse = GetResponse(apiLink);
        if (jsonResponse.Equals("ERROR"))
        {
            resultsList.Add(false);
        }
        else
        {
            resultsList.Add(true);
            LeaderboardAPI info = JsonUtility.FromJson<LeaderboardAPI>(jsonResponse);
            resultsList.Add(info);
        }
        return resultsList;
    }

    public ArrayList GetChallengesTest() //dummy function to return json for challenges api - for development testing purposes.
    {
        ArrayList resultsList = new ArrayList();
        string apiLink = String.Format("/challenge/{0}", playerInfo.Data.username);

        string jsonResponse = GetResponse(apiLink);
        if (jsonResponse.Equals("ERROR"))
        {
            resultsList.Add(false);
        }
        else
        {
            resultsList.Add(true);
            Debug.Log("RECEIVED: " + jsonResponse);
            resultsList.Add(jsonResponse); 
        }
        return resultsList;

    }
    public ArrayList GetChallenges() //actual function for challenge api access.
    {

        ArrayList resultsList = new ArrayList();
        string apiLink = String.Format("/challenge/{0}/list", playerInfo.Data.username);
        string jsonResponse = GetResponse(apiLink); 
        if (jsonResponse.Equals("ERROR"))
        {
            resultsList.Add(false);
        }
        else
        {
            resultsList.Add(true);
            ChallengesAPI info = JsonUtility.FromJson<ChallengesAPI>(jsonResponse); //add this and the one below once challenge api response is fixed.
            resultsList.Add(info);
        }
        return resultsList;
    }


    public ArrayList PostLogin(string username)
    {
        ArrayList arr = new ArrayList();
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        LoginResponseAPI loginData = new LoginResponseAPI();
        string result = "";
        LoginAPI logUser = new LoginAPI();
        logUser.name = username;
        logUser.deviceID = deviceID;
        result = Post2("/account/login", logUser);
        try
        {
            loginData = JsonUtility.FromJson<LoginResponseAPI>(result);
            arr.Add(result);
            arr.Add(loginData);

        }
        catch(Exception e)
        {
            
            if (result.Contains("This account may have existed"))
            {
                result = "INVALID";
            }
            else
            {
                Debug.Log("[PostLogin()]: Error");
                result = "ERROR";
            }
            arr.Add(result);
        }
        return arr;
    }

    public ArrayList PostCreateChallenge(string date_time, string name, string type, int level)
    {
        ArrayList arr = new ArrayList();
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        CreateChallengeResponseAPI createResponse = new CreateChallengeResponseAPI();
        string result = "";
        CreateChallengeEntryAPI challengeRequest = new CreateChallengeEntryAPI(date_time, name, type, level);
        result = Post2("/challenge/", challengeRequest);
        try
        {
            createResponse = JsonUtility.FromJson<CreateChallengeResponseAPI>(result);
            arr.Add(result);
            arr.Add(createResponse);

        }
        catch (Exception e)
        {
            
            if (result.Contains("User must finish that level before challenging the other"))
            {
                result = "INVALID";
            }
            else
            {
                Debug.Log("[PostCreateChallenge()]: Error");
                result = "ERROR";
            }
            arr.Add(result);
        }
        return arr;
    }
    public ArrayList PutStartChallenge(string username, string challengeId)
    {   
        ArrayList arr = new ArrayList();
        string result = "";
        String apiLink = String.Format("/challenge/start/{0}", challengeId);
        result = Put(apiLink, new StartChallengeEntryAPI(playerInfo.Data.username));
        StartChallengeResponseAPI challengeResponse = new StartChallengeResponseAPI();
        try
        {
            challengeResponse = JsonUtility.FromJson<StartChallengeResponseAPI>(result);
            arr.Add(result);
            arr.Add(challengeResponse);
        }
        catch (Exception e)
        {
            Debug.Log("[PutStartChallenge()]: Error");
            if (result.Contains("This challenge has been taken"))
            {
                result = "INVALID";
            }
            else
            {
                result = "ERROR";
            }
            arr.Add(result);
        }
        return arr;
    }

    public ArrayList PutUpdateChallenge(string username, int timing, string challengeId)
    {
        ArrayList arr = new ArrayList();
        string result = "";
        String apiLink = String.Format("/challenge/finish/{0}", challengeId);
        result = Put(apiLink, new UpdateChallengeEntryAPI(playerInfo.Data.username,timing));
        UpdateChallengeResponseAPI challengeResponse = new UpdateChallengeResponseAPI();
        try
        {
            challengeResponse = JsonUtility.FromJson<UpdateChallengeResponseAPI>(result);
            arr.Add(result);
            arr.Add(challengeResponse);
        }
        catch (Exception e)
        {
            Debug.Log("[PutUpdateChallenge()]: Error");
            if (result.Contains("Other user has taken this challenge"))
            {
                result = "INVALID";
            }
            else
            {
                result = "ERROR";
            }
            arr.Add(result);
        }
        return arr;
    }

    public ArrayList PostLeaderboard(string username, string leaderboardType, StandardEntryAPI inputEntry)
    {
         //see challengesscript.cs for sample implementation (search "test").
        ArrayList arr = new ArrayList();
        LoginResponseAPI leaderboardData = new LoginResponseAPI();
        this.leadboardFlag = false;
        string result = "";
        result = Post2(String.Format("/score/{0}/{1}",username,leaderboardType), inputEntry);
        leaderboardData = JsonUtility.FromJson<LeaderboardResponseAPI>(result).info;
        arr.Add(result);
        arr.Add(leaderboardData);
        return arr;
    }


    public ArrayList PostAttempts(string username, AttemptList aList)
    {
        ArrayList arr = new ArrayList();
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        LoginResponseAPI loginData = new LoginResponseAPI();
        string result = "";
        string apiLink = String.Format("/attempt/{0}", username);
        result = Post2(apiLink, aList);
        try
        {
            loginData = JsonUtility.FromJson<LoginResponseAPI>(result);
            arr.Add(result);
            arr.Add(loginData);

        }
        catch (Exception e)
        {

            if (result.Contains("Cannot read property 'attempts' of null"))
            {
                result = "INVALID";
            }
            else
            {
                Debug.Log("[PostAttempts()]: Error");
                result = "ERROR";
            }
            arr.Add(result);
        }
        return arr;
    }
    public void OnApplicationFocus(bool focus)
    {
        if (focus) //user loads the app 
        {
            Debug.Log("Focus is TRUE");
        }
        else //focus=false means the user exits the app (back/home button)
        {
            Debug.Log("Focus is FALSE");
            playerInfo.LoadDataFromPlayerPref(playerInfo.Data.username); //load latest data from playerpref
            if (playerInfo.Data.attempts.Count > 0)
            {
                Debug.Log("[OnApplicationFocus()]: pending attempts to be posted in attemptsList: " + playerInfo.Data.attempts.Count.ToString());
                AttemptList aList = new AttemptList(); //Must use [Serializable] AttemptList class for posting...
                foreach (AttemptEntry item in playerInfo.Data.attempts)
                {
                    aList.Add(item);
                }
                //do postAttempts()...
                ArrayList arr = PostAttempts(playerInfo.Data.username, aList); 
                if (!arr[0].Equals("ERROR") && !arr[0].Equals("INVALID"))
                {
                    Debug.Log("[OnApplicationFocus()]: Successful post of attempts to server!");
                    playerInfo.Data.attempts.Clear(); //reset attempts
                    playerInfo.SaveDataToPlayerPref(); //save to playerprefs
                }
                else
                {
                    Debug.Log("[OnApplicationFocus()]: Unsuccessful post of attempts to server - ERROR!");
                }
            }
            else
            {
                Debug.Log("[OnApplicationFocus()]: No pending attempts...");
            }

        }
    }
    
}

[Serializable]
public class ChuckNorris
{
    public string id;
    public string value;
    
}
[Serializable]
public class LoginAPI
{
    public string name;
    public string deviceID;
}

[Serializable]
public class BoardEntryAPI
{
    public int score;
    public string name;
    
}
[Serializable]
public class LeaderboardAPI
{
    public List<BoardEntryAPI> board;
}
[Serializable]
public class StartChallengeEntryAPI
{
    public string name;
    public StartChallengeEntryAPI(string name)
    {
        this.name = name;
    }
}

[Serializable]
public class StartChallengeResponseAPI
{
    public string status;
    public StartChallengeEntryAPI info;
}
[Serializable]
public class ChallengeEntryAPI
{
    public string state;
    public int recvTime;
    public string _id;
    public string date_time;
    public string senderName;
    public int senderTime;
    public int level;
    public string type;
}
[Serializable]
public class ChallengesAPI
{
    public List<ChallengeEntryAPI> challenges = new List<ChallengeEntryAPI>();
}

[Serializable]
public class StandardEntryAPI //can use for Standard/Weekly/All-Time
{
    public int score;
    public int time;
    //public string _id;
    public string date_time;
    public int level;

    public StandardEntryAPI(int score, int time, string date_time, int level)
    {
        this.score = score;
        this.time = time;
        this.date_time = date_time;
        this.level = level;
    }
}

[Serializable]
public class LoginResponseAPI
{
    public string _id;
    public int levelUnlock;
    public string name;
    public string deviceID;
    
    public List<AttemptEntry> attempts = new List<AttemptEntry>();
    public List<StandardEntryAPI> standard = new List<StandardEntryAPI>();
    public List<StandardEntryAPI> special = new List<StandardEntryAPI>();
    public List<StandardEntryAPI> weekly = new List<StandardEntryAPI>();
    public string createdAt;
    public string updatedAt;
    public int __v;
    //public List<ChallengeEntryAPI> challenges;

}

[Serializable]
public class LeaderboardResponseAPI
{
    public string status;
    public LoginResponseAPI info;
}

[Serializable]

public class UpdateChallengeEntryAPI
{
    public string name;
    public int time;

    public UpdateChallengeEntryAPI(string name, int time)
    {
        this.name = name;
        this.time = time;
    }
}

[Serializable]
public class UpdateChallengeResponseAPI
{
    public string state;
    public string recvName;
    public int recvTime;
    public string _id;
    public string date_time;
    public string senderName;
    public string senderTime;
    public int level;
    public string type;
}


[Serializable]
public class CreateChallengeEntryAPI
{
    public string date_time;
    public string senderName;
    public string type;
    public int level;

    public CreateChallengeEntryAPI(string date_time, string senderName, string type, int level)
    {
        this.date_time = date_time;
        this.senderName = senderName;
        this.type = type;
        this.level = level;
    }
}

[Serializable]
public class CreateChallengeResponseAPI
{
    public string state;
    public string recvName;
    public int recvTime;
    public string _id;
    public string date_time;
    public string senderName;
    public string senderTime;
    public int level;
    public string type;
}
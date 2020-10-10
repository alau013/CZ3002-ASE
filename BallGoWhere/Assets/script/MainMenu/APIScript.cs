﻿using System;
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
    public string localHostIp; //SET THIS ONLY FROM UNITY!
    public GameObject PrefObject;
    private PlayerPrefUI playerInfo;
    private bool loginFlag = false;
    public void OnEnable()
    {
        playerInfo = PrefObject.GetComponent<PlayerPrefUI>();
        
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
                    jsonResponse = "SUCCESS";
                    //jsonResponse = contentStr; //replace with this when the api response has a flag for success/username already taken. you will need to modify LoginMenu.cs accordingly as well.
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
   /*
    public IEnumerator Post(string apiLink,object dataObject) 
    {
        apiLink = this.localHostIp + apiLink;
        string jsonStr = JsonUtility.ToJson(dataObject);
        
        Debug.Log("JSON...");
        Debug.Log(jsonStr.ToString());

        byte[] bytePostData = Encoding.UTF8.GetBytes(jsonStr);
        var request = new UnityWebRequest(apiLink,"POST");
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bytePostData);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        
        if(!request.isNetworkError)
        {
            Debug.Log("Data upload success!");
            loginFlag = true;
        }
        else
        {
            Debug.Log("Data upload error!");
            loginFlag = false;
        }

    }
   */
   
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


    public string PostLogin(string username)
    {
        this.loginFlag = false;
        string result = "";
        LoginAPI logUser = new LoginAPI();
        logUser.name = username;
        //StartCoroutine(Post("/account/login", logUser));
        result = Post2("/account/login", logUser);
        return result;
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
            
            /* //test code for uploading attempts to DB
            AttemptEntry a1 = new AttemptEntry();
            a1.date_time =  "2020-10-07";
            a1.point = 24;
            a1.level = 1;
            AttemptList aList = new AttemptList();
            aList.attempts.Add(a1);

            Debug.Log("Attempts: ");
            Debug.Log(aList.attempts);
            StartCoroutine(Post("/attempt/jaslyn", aList)); //important to use this
            */

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



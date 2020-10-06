using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
using TMPro;
using System.Globalization;
using UnityEngine.Rendering;
using System.Threading.Tasks;

public class APIScript : MonoBehaviour
{
    public int timeOut = 2;
    public string localHostIp = "192.168.1.15";
    public ChuckNorris GetChuckling()
    {
        string apiLink = "https://api.chucknorris.io/jokes/random";
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", CityId, API_KEY));
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiLink);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        ChuckNorris info = JsonUtility.FromJson<ChuckNorris>(jsonResponse);
        Debug.Log("[GetChuckling()]...");
        Debug.Log(info.value);
        return info;
    }

    /*public LeaderboardAPI GetBoard()
    {

        String dateStr = System.DateTime.Now.ToString(@"yyyy-MM-dd");
        string apiLink = String.Format("http://localhost:3000/score/leaderboard/weekly?date=\"{0}\"",dateStr);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiLink);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        LeaderboardAPI info = JsonUtility.FromJson<LeaderboardAPI>(jsonResponse);
        return info;

    }
    */

    public async Task<ArrayList> GetLeaderboard()
    {
        try
        {
            ArrayList resultsList = new ArrayList();
            Boolean flag;
            String dateStr = System.DateTime.Now.ToString(@"yyyy-MM-dd");
            string apiLink = String.Format("http://{0}:3000/score/leaderboard/weekly?date=\"{1}\"", this.localHostIp,dateStr);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiLink);
            HttpWebResponse response;
            //HttpWebResponse response =(HttpWebResponse) request.GetResponse();
            //return resultsList;
            
            var task = Task.Run(async () => await request.GetResponseAsync());
            if (task.Wait(TimeSpan.FromSeconds(this.timeOut)))
            {
                response = (HttpWebResponse)task.Result;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string jsonResponse = reader.ReadToEnd();
                LeaderboardAPI info = JsonUtility.FromJson<LeaderboardAPI>(jsonResponse);
                flag = true;
                resultsList.Add(flag);
                resultsList.Add(info);
                return resultsList;
            }

            else
            {
                Debug.Log("[APIScript.cs GetLeaderboard()]: ERROR!");
                flag = false;
                resultsList.Add(flag);
                return resultsList;
                throw new Exception("Timed out");

            }

        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;

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


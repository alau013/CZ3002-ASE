using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
using TMPro;

public class APIScript : MonoBehaviour
{
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
}

[Serializable]
public class ChuckNorris
{
    public string id;
    public string value;
    
}


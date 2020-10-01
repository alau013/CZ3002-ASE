using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefUI : MonoBehaviour
{
    public string strPlayers = ""; // "1 2 3 5 6 7">> string of valid playerID. Split by " " to iterate through it..
    public string username;
    public int streak = 0;
    public float dailyPlay = 0; //total playtime for the day, in mins
    public int level =3; //highest unlocked level
    public int dailyAttempts = 0; //no. of attempted levels today
    public string lastActive; //datetime string from last level attempt

    

    public void Save()
    {
        string playerID; //might change as players are added/removed. only used for internal identification purposes.

        if (PlayerPrefs.HasKey("NumPlayers") == false)
        {
            PlayerPrefs.SetInt("NumPlayers", 1);
            PlayerPrefs.SetString("StrPlayers", "1");
            playerID = "1";
        }
        else
        {
            PlayerPrefs.SetInt("NumPlayers", PlayerPrefs.GetInt("NumPlayers")+1);
            playerID = PlayerPrefs.GetInt("NumPlayers").ToString();
            PlayerPrefs.SetString("StrPlayers", PlayerPrefs.GetString("StrPlayers") + " " + playerID);
        }

        strPlayers = PlayerPrefs.GetString("StrPlayers");
        PlayerPrefs.SetString(playerID, username); //1 ahHuat08
        PlayerPrefs.SetInt(username, 1); //ahHuat08 : 1 (binary flag for quick check if username is saved)
        PlayerPrefs.SetInt(username + "_Streak", streak); //ahHuat08_Streak : 7
        PlayerPrefs.SetFloat(username + "_dailyPlay", dailyPlay); //ahHuat08_DailyPlay : 32.3 mins
        PlayerPrefs.SetInt(username + "_dailyAttempts", dailyAttempts); //ahHuat08_DailyAttempts : 5
        PlayerPrefs.SetInt(username + "_Level", level); //ahHuat08_Level : 3
        PlayerPrefs.SetString(username + "_LastActive", lastActive); //ahHuat08_LastActive : "1/12/2020 12:00:00 AM"
    }

    public void Load(string playerName)
    {
        if (PlayerPrefs.HasKey(playerName))
        {
            username = playerName;
            streak = PlayerPrefs.GetInt(playerName + "_Streak");
            dailyPlay = PlayerPrefs.GetFloat(playerName + "_Float");
            level = PlayerPrefs.GetInt(playerName + "_Level");
            lastActive = PlayerPrefs.GetString(playerName + "_LastActive");
        }
        else
        {
            print("[PlayerPrefUI]: " + "BEEEP boop BEEP error detected! Invalid playername!");
        }
    }
}

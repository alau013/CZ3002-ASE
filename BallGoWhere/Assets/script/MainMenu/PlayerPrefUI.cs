using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.UI;
using UnityEngine;


public class PlayerPrefUI : MonoBehaviour
{
    public PlayerData Data = new PlayerData();
    public DeviceUsernames users= new DeviceUsernames();

    public void SetData(PlayerData inputData)
    {
        this.Data = inputData;
    }

    public void LoadDataFromPlayerPref(string nameStr)
    {
        if (PlayerPrefs.HasKey(nameStr))
        {
            string jsonStr = PlayerPrefs.GetString(nameStr);
            this.SetData(JsonUtility.FromJson<PlayerData>(jsonStr));
        }
        else
        {
            Debug.Log("[PlayerPrefUI]: " + "BEEEP boop BEEP error detected! Invalid playername load attempt!");
        }

    }

    public void SaveDataToPlayerPref()
    {
        if (PlayerPrefs.HasKey("Usernames"))
        {
            this.users = JsonUtility.FromJson<DeviceUsernames>(PlayerPrefs.GetString("Usernames"));
            
        }

        if (this.users.nameList.Contains(this.Data.username) == false)
        {
            this.users.nameList.Add(this.Data.username);
        }
        PlayerPrefs.SetString("Usernames", JsonUtility.ToJson(this.users)); //save to list of usernames on device
        PlayerPrefs.SetString(this.Data.getUsername(), this.Data.ExportToJson()); //save user's data to device
    }

    public List<string> getNameList()
    {
        List<string> newList = new List<string>();
        if (PlayerPrefs.HasKey("Usernames"))
        {
            this.users = JsonUtility.FromJson<DeviceUsernames>(PlayerPrefs.GetString("Usernames"));
            newList = this.users.nameList;

        }
        
        return newList;
    }


}
[Serializable]
public class DeviceUsernames
{
    public List<string> nameList = new List<String>();
}

[Serializable]
public class AttemptEntry
{
    public string date_time;
    public int point;
    public int level;

    public AttemptEntry(string date_time,int point, int level)
    {
        this.date_time = date_time;
        this.point = point;
        this.level = level;
    }
}

[Serializable]
public class AttemptList
{
    public List<AttemptEntry> attempts = new List<AttemptEntry>();
    public string ExportToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public AttemptList LoadFromJson(string jsonStr)
    {
        return JsonUtility.FromJson<AttemptList>(jsonStr);
    }
}
[Serializable]
public class PlayerData
{
    public string username;
    public int streak = 0;
    public float dailyPlay = 0; //total playtime for the day, in mins
    public int level = 3; //highest unlocked level
    public int dailyAttempts = 0; //no. of attempted levels today
    public string lastActive; //datetime string from last login/attempt
    public List<AttemptEntry> attempts = new List<AttemptEntry>();
    public string ExportToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public PlayerData LoadFromJson(string jsonStr)
    {
        return JsonUtility.FromJson<PlayerData>(jsonStr);
    }

    public void setUsername(string playerName)
    {
        this.username = playerName;

    }

    public string getUsername()
    {
        return this.username;
    }

    public void addStreak()
    {
        this.streak += 1;
    }

    public void resetStreak()
    {
        this.streak = 0;
    }

    public int getStreak()
    {
        return this.streak;
    }

    public float getDailyPlay()
    {
        return this.dailyPlay;
    }

    public void resetDailyPlay()
    {
        this.dailyPlay = 0;
    }

    public int getLevel()
    {
        return this.level;
    }

    public void addLevel(int num)
    {
        this.level += num;
    }


    public void addAttempt(AttemptEntry inputAttempt)
    {
        this.attempts.Add(inputAttempt);
    }
    public void deleteAllAttempts()
    {
        this.attempts.Clear();
    }

    public int getDailyAttempts()
    {
        return this.dailyAttempts;
    }

    public void addDailyAttempts()
    {
        this.dailyAttempts += 1;
    }

    public void resetDailyAttempts()
    {
        this.dailyAttempts = 0;
    }

    public void updateLastActive()
    {
        this.lastActive = System.DateTime.Now.ToString();
    }

    public string getLastActive()
    {
        return this.lastActive;
    }
}
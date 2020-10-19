using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine;


public class PlayerPrefUI : MonoBehaviour
{
    public PlayerData Data = new PlayerData();


    public DeviceUsernames users = new DeviceUsernames();


    private static bool created = false;


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
            //this.Data.loadDailyPlayList();
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
    public int time;
    public int level;
    public string type;

    public AttemptEntry()
    {
    }

    public AttemptEntry(string date_time, int point, int time, int level, string type)
    {
        this.date_time = date_time;
        this.point = point;
        this.time = time;
        this.level = level;
        this.type = type;

    }
    public AttemptEntry(string date_time, int point, int level)
    { //for testing purposes. Alan to update the AttemptEntry() in Goal.Cs to include time and type(like above).
        this.date_time = date_time;
        this.point = point;
        this.time = 10;
        this.level = level;
        this.type = "standard";
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

    public void Add(AttemptEntry inputEntry)
    {
        attempts.Add(inputEntry);
    }

    public int Count()
    {
        return attempts.Count;
    }

}

[Serializable]
public class DataEntry{
    public string key;
    public int value;
    public DataEntry()
    {

    }

    public DataEntry(string key, int value)
    {
        this.key = key;
        this.value = value;
    }

    public bool CompareKey(string compareKey)
    {
        bool result = false;
        if (compareKey.Equals(this.key))
        {
            result = true;
        }
        return result;
    }

    public string GetKey()
    {
        return this.key;
    }
    public int GetValue()
    {
        return this.value;
    }

    public void SetValue(int newValue)
    {
        this.value = newValue;
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
    public String lastActive; //datetime string from last login/attempt
    public List<AttemptEntry> attempts = new List<AttemptEntry>();
    public List<StandardEntryAPI> standard = new List<StandardEntryAPI>();
    public List<StandardEntryAPI> special = new List<StandardEntryAPI>();
    public List<StandardEntryAPI> weekly = new List<StandardEntryAPI>();
    public ArrayList challengeHolder = new ArrayList();
    public List<DataEntry> dailyPlayList = new List<DataEntry>();
    public string jsonDailyPlayList;
    //public Dictionary<DateTime, int> dailyPlayDict = new Dictionary<DateTime, int>();
    public Dictionary<DateTime, List<AttemptEntry>> attemptsDict = new Dictionary<DateTime, List<AttemptEntry>>();

    public string ExportToJson()
    {
        if (this.dailyPlayList.Count > 0)
        {
            //this.jsonDailyPlayList = JsonUtility.ToJson(this.dailyPlayList);
            //Debug.Log("[ExportToJson()] Saving first to jsonDailyPlayList: " + jsonDailyPlayList);
        }
        
        return JsonUtility.ToJson(this);
    }
    public PlayerData LoadFromJson(string jsonStr)
    {
        return JsonUtility.FromJson<PlayerData>(jsonStr);
    }

    /*
    public List<DataEntry> loadDailyPlayList()
    {
        if (!jsonDailyPlayList.Equals(""))
        {
            return JsonUtility.FromJson<List<DataEntry>>(this.jsonDailyPlayList);
        }
        else
        {
            return new List<DataEntry>();
        }
    }*/
    public void UpdateAttemptsDict(int point, int time, int level, string type) //update using raw data: points and level.
    {
        //Update attemptsdict (for use locally)
        //DateTime currDate = System.DateTime.Now.Date;
        DateTime currDate = System.DateTime.Now;
        string dateTimeStr = currDate.ToString("MM/dd/yyyy HH:mm:ss");
        currDate = currDate.Date;
        //attempts.Add(new AttemptEntry(dateTimeStr, point, time, level, type)); //Add to attemptsList (waiting list to post to server)
        if (!attemptsDict.ContainsKey(currDate))
        {
            attemptsDict[currDate] = new List<AttemptEntry>();
        }
        attemptsDict[currDate].Add(new AttemptEntry(currDate.ToString(@"yyyy-MM-dd"), point, time, level, type));
    }
    public void UpdateAttemptsDict(AttemptEntry inputAttempt) //update using AttemptEntry object.
    {
        DateTime currDate = System.DateTime.Now.Date;
        if (!attemptsDict.ContainsKey(currDate))
        {
            attemptsDict[currDate] = new List<AttemptEntry>();
        }
        attemptsDict[currDate].Add(inputAttempt);

        foreach (var item in attemptsDict.Keys)
        {
            Debug.Log("K: " + item.ToString());

        }
    }

    public void UpdateDailyPlayDict(int newTiming)
    {
        //string dateStr = System.DateTime.Now.Date.ToString(@"yyyy-MM-dd");
        DateTime currDate = System.DateTime.Now.Date;
        String dateStr = currDate.ToString();
        bool foundFlag = false;
        foreach (DataEntry item in dailyPlayList)
        {
            if (item.CompareKey(dateStr))
            {
                item.SetValue(item.GetValue() + newTiming);
                foundFlag = true;
            }   
        }

        if (!foundFlag)
        {
            dailyPlayList.Add(new DataEntry(dateStr, newTiming));
            Debug.Log("[UpdateDailyPlayDict()]: Adding " + dateStr + ", " + newTiming.ToString());
        }

        if (dailyPlayList.Count > 7)
        {
            dailyPlayList.RemoveAt(0); //First in first out.
        }

       
        this.UpdateLastActive();

    }

    public void UpdateDailyPlayDict(DateTime currDate, int newTiming)
    {
        //string dateStr = System.DateTime.Now.Date.ToString(@"yyyy-MM-dd");
        String dateStr = currDate.Date.ToString();
        bool foundFlag = false;
        foreach (DataEntry item in dailyPlayList)
        {
            if (item.CompareKey(dateStr))
            {
                item.SetValue(item.GetValue() + newTiming);
                foundFlag = true;
            }
        }

        if (!foundFlag)
        {
            dailyPlayList.Add(new DataEntry(dateStr, newTiming));
        }

        if (dailyPlayList.Count > 7)
        {
            dailyPlayList.RemoveAt(0); //First in first out.
        }


        this.UpdateLastActive();
    }

    public int GetDailyPlayFromList(DateTime givenDate)
    {
        int result = 0;
        String dateStr = givenDate.Date.ToString();
        foreach (DataEntry item in dailyPlayList)
        {
            if (item.CompareKey(dateStr))
            {
                result = item.GetValue();
            }
        }

        return result;
    }
    public void UpdateLastActive() //call this each time the player completes a level.
    {
        DateTime currDate = System.DateTime.Now;
        string currDateStr = currDate.Date.ToString();
        
        if (!currDateStr.Equals(this.lastActive)) //if current date is not equal to last active date..check if streak is broken.
        {
            if (!this.lastActive.Equals(""))
            {
                DateTime lastActiveDate = DateTime.Parse(this.lastActive);
                TimeSpan value = currDate.Subtract(lastActiveDate);
                if (value.Days > 1)
                {
                    this.resetStreak();
                }
                else
                {
                    this.addStreak();
                }
            }

        }

        this.lastActive = currDateStr;

    }
    public void LoadLoginData(LoginResponseAPI loginData)
    {
        this.username = loginData.name;
        this.level = loginData.levelUnlock;
        this.standard = loginData.standard;
        this.special = loginData.special;
        this.weekly = loginData.weekly;
        //skip attempts as it is not required
    }


    public int getLeaderboardScore(string leaderboardType, int levelNum)
    {
        Dictionary<string, List<StandardEntryAPI>> typeDict = new Dictionary<string, List<StandardEntryAPI>>();
        typeDict.Add("standard", this.standard);
        typeDict.Add("special", this.special);
        typeDict.Add("weekly", this.weekly);

        int result = -1;
        Debug.Log("current count in playerInfo.data: " + typeDict[leaderboardType].Count);
        if (typeDict[leaderboardType].Count > 0)
        {
            for (int i = 0; i < typeDict[leaderboardType].Count; i++)
            {
                Debug.Log("[entries in leaderboard scores]:");
                Debug.Log(typeDict[leaderboardType][i].level + "" + typeDict[leaderboardType][i].score);
                if (typeDict[leaderboardType][i].level.Equals(levelNum))
                {
                    result = typeDict[leaderboardType][i].score;
                    Debug.Log("[getLeaderboardScore()]: Found result for level " + typeDict[leaderboardType][i].level + " score: " + typeDict[leaderboardType][i].score);
                    break;
                }
            }
        }
        return result;
    }

    
    public void setStandard(List<StandardEntryAPI> someList)
    {
        this.standard = someList;
    }

    public List<StandardEntryAPI> getStandard()
    {
        return this.standard;
    }
    public void setSpecial(List<StandardEntryAPI> someList)
    {
        this.special = someList;
    }

    public List<StandardEntryAPI> getSpecial()
    {
        return this.special;
    }

    public void setWeekly(List<StandardEntryAPI> someList)
    {
        this.weekly = someList;
    }

    public List<StandardEntryAPI> getWeekly()
    {
        return this.weekly;
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

    public void addDailyPlay(float dailyPlayTime)
    { //ignore this...legacy function no longer in use...
        this.dailyPlay = dailyPlay + dailyPlayTime;
        
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
        this.attempts.Add(inputAttempt); //add to waiting list (attempts) for upload to server
        UpdateAttemptsDict(inputAttempt); //save to local data (playerprefs) for data viz in dashboard
        UpdateDailyPlayDict(inputAttempt.time); //save to local data (playerprefs) for data viz in dashboard
        
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

    

    public String getLastActive()
    {
        return this.lastActive;
    }
}
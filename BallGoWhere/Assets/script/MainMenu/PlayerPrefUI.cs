using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine;

/*
 * This class implements PlayerPrefUI.cs This provides the data layer for the app to communicate with the PlayerPrefs file (local data on the device).
 * An empty game object in each scene will link to this PlayerPrefUI.cs to allow access to the PlayerPrefs file.
 * @author Tay Jaslyn
 * 
 */
public class PlayerPrefUI : MonoBehaviour
{
    public PlayerData Data = new PlayerData(); //PlayerData class object which contains the user data.


    public DeviceUsernames users = new DeviceUsernames();


    private static bool created = false;


    public void SetData(PlayerData inputData)
    {
        this.Data = inputData;
    }

    public void LoadDataFromPlayerPref(string nameStr)
    {
        if (PlayerPrefs.HasKey(nameStr)) //loads user data from PlayerPrefs file. Refer to the PlayerData class in this script for the user data.
        {
            string jsonStr = PlayerPrefs.GetString(nameStr);
            Debug.Log("LoadDataFromPlayerPrefs: " + jsonStr);
            this.SetData(this.Data.LoadFromJson(jsonStr));
        }
        else
        {
            Debug.Log("[PlayerPrefUI]: " + "BEEEP boop BEEP error detected! Invalid playername load attempt!");
        }

    }

    public void SaveDataToPlayerPref() //Saves current user data to the PlayerPrefs file.
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
        Debug.Log("Saving: " + this.Data.ExportToJson());
        PlayerPrefs.SetString(this.Data.getUsername(), this.Data.ExportToJson()); //save user's data to device
    }

    public List<string> getNameList() //Retrieve list of existing users from the PlayerPrefs file.
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

//Serializable classes for saving/retrieval of data from PlayerPrefs 
//This is because PlayerPrefs does not allow the direct saving of class objects - only integers/floats/strings. Hence, we will first
//serialize the class objects into json strings to save them.

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
public class DailyAttempts
{
    public string key;
    public List<AttemptEntry> aList;
    public DailyAttempts()
    {

    }

    public DailyAttempts(string key, List<AttemptEntry> aList)
    {
        this.key = key;
        this.aList = aList;
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
    public List<AttemptEntry> GetList()
    {
        return this.aList;
    }

    public void AddToList(AttemptEntry inputEntry)
    {
        this.aList.Add(inputEntry);
    }

}

[Serializable]
public class PlayerData //This class contains the user data that is saved to/retrieved from the PlayerPrefs file.
{
    public string username;
    public int streak = 0;
    public int level = 3; //highest unlocked level
    public String lastActive; //datetime string from last login/attempt
    public List<AttemptEntry> attempts = new List<AttemptEntry>();
    public List<StandardEntryAPI> standard = new List<StandardEntryAPI>();
    public List<StandardEntryAPI> special = new List<StandardEntryAPI>();
    public List<StandardEntryAPI> weekly = new List<StandardEntryAPI>();
    public ArrayList challengeHolder = new ArrayList();
    public List<DataEntry> dailyPlayList = new List<DataEntry>();
    public List<DailyAttempts> dailyAttemptsList = new List<DailyAttempts>();
    public Dictionary<DateTime, List<AttemptEntry>> attemptsDict = new Dictionary<DateTime, List<AttemptEntry>>();

    public string ExportToJson()
    {
        
        return JsonUtility.ToJson(this);
    }
    public PlayerData LoadFromJson(string jsonStr)
    {
        return JsonUtility.FromJson<PlayerData>(jsonStr);
    }

    public int GetDailyAttemptsCount() //retrieves the count of attempts for the day.
    {
        int result = 0;
        String currDateStr = System.DateTime.Now.Date.ToString();
        foreach (DailyAttempts item in this.dailyAttemptsList)
        {
            if (item.CompareKey(currDateStr))
            {
                result = item.aList.Count;
                break;
            }
        }

        return result;
    }

    public double GetDailyPlayMins() //retrieves the sum of minutes in gameplay for the day.
    {
        double result = 0;
        String currDateStr = System.DateTime.Now.Date.ToString();
        foreach (DailyAttempts item in this.dailyAttemptsList)
        {
            if (item.CompareKey(currDateStr))
            {
                foreach (AttemptEntry aEntry in item.aList)
                {
                    result += aEntry.time;
                }
                break;
            }
        }

        result = Math.Round(result / 60,2);

        return result;
    }

    public ArrayList GetDailyPlayDates() 
    {
        ArrayList arr = new ArrayList();

        foreach (DailyAttempts item in this.dailyAttemptsList)
        {
            arr.Add(item.key);
        }

        return arr;

    }
    public ArrayList GetAllDailyPlayMins() //Returns list of the sum of gameplay in mins for each day
    {
        ArrayList arr = new ArrayList();
        
        foreach (DailyAttempts item in this.dailyAttemptsList)
        {
            double result = 0;
            foreach (AttemptEntry aEntry in item.aList)
            {
                result += aEntry.time;
            }
            result = Math.Round(result / 60, 2);
            arr.Add(result);
        }

        return arr;
    }
    public void UpdateAttemptsDict(int point, int time, int level, string type) //update using raw data: points and level.
    {
        //Update attemptsdict (for use locally)
        DateTime currDate = System.DateTime.Now;
        string dateTimeStr = currDate.ToString("MM/dd/yyyy HH:mm:ss");
        currDate = currDate.Date;
        bool foundFlag = false;

        foreach (DailyAttempts item in this.dailyAttemptsList)
        {
            if (item.CompareKey(currDate.ToString()))
            {
                item.AddToList(new AttemptEntry(dateTimeStr, point, time, level, type));
                foundFlag = true;
            }
        }

        if (!foundFlag)
        {
            List<AttemptEntry> aList = new List<AttemptEntry>();
            aList.Add(new AttemptEntry(dateTimeStr, point, time, level, type));
            this.dailyAttemptsList.Add(new DailyAttempts(currDate.ToString(), aList));
        }

        if (dailyAttemptsList.Count > 7)
        {
            dailyAttemptsList.RemoveAt(0); //First in first out.
        }

    }
    public void UpdateAttemptsDict(AttemptEntry inputAttempt) //update using AttemptEntry object.
    {
        //Update attemptsdict (for use locally)
        DateTime currDate = System.DateTime.Now;
        string dateTimeStr = currDate.ToString("MM/dd/yyyy HH:mm:ss");
        currDate = currDate.Date;
        bool foundFlag = false;

        foreach (DailyAttempts item in this.dailyAttemptsList)
        {
            if (item.CompareKey(currDate.ToString()))
            {
                item.AddToList(inputAttempt);
                foundFlag = true;
            }
        }

        if (!foundFlag)
        {
            List<AttemptEntry> aList = new List<AttemptEntry>();
            aList.Add(inputAttempt);
            this.dailyAttemptsList.Add(new DailyAttempts(currDate.ToString(), aList));
        }

        if (dailyAttemptsList.Count > 7)
        {
            dailyAttemptsList.RemoveAt(0); //First in first out.
        }
    }

    public void UpdateDailyPlayDict(int newTiming) //Updates dictionary of gameplay mins per day, using current date.
    {
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

    public void UpdateDailyPlayDict(DateTime currDate, int newTiming)//Updates dictionary of gameplay mins per day, using given date.
    {
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

    public int GetDailyPlayFromList(DateTime givenDate) //Gets sum of daily play minutes for given date.
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
    public void UpdateLastActive() //Update last active date of gameplay.
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
                    this.resetStreak(); //reset streak to 0.
                }
                else
                {
                    this.addStreak(); //adds on to streak.
                }
            }

        }

        this.lastActive = currDateStr;

    }
    public void LoadLoginData(LoginResponseAPI loginData) //Loads login response data from the API
    {
        this.username = loginData.name;
        this.level = loginData.levelUnlock;
        this.standard = loginData.standard; //Highest scores for each level (all-time)
        this.special = loginData.special; //Highest scores for each level (special)
        this.weekly = loginData.weekly; //Highest scores for each level (weekly)
    }


    public int getLeaderboardScore(string leaderboardType, int levelNum) //Retrieves the highscore for the leaderboard type and level number
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
        //this.dailyPlay = dailyPlay + dailyPlayTime;
        
    }

    public int getLevel()
    {
        return this.level;
    }

    public void addLevel(int num)
    {
        this.level += num;
    }

    public void addAttempt(AttemptEntry inputAttempt) //This is called after each gameplay to save the relevant data.
    {
        this.attempts.Add(inputAttempt); //add to waiting list (attempts) for upload to server
        UpdateAttemptsDict(inputAttempt); //save to local data (playerprefs) for data viz in dashboard
        UpdateDailyPlayDict(inputAttempt.time); //save to local data (playerprefs) for data viz in dashboard
        
    }

    public void deleteAllAttempts()
    {
        this.attempts.Clear();
    }

    

    public String getLastActive()
    {
        return this.lastActive;
    }
}
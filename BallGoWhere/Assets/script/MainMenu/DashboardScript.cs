using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class DashboardScript : MonoBehaviour
{
    public GameObject APIObject;
    public GameObject PrefObject;
    public GameObject MainScreen;
    public GameObject DashboardScreen;
    public GameObject Viz;
    public GameObject VizCaptionObject;
    public GameObject Stats;
   // public TMP_Text VizCaption;
    public TMP_Text StatsDate;
    public TMP_Text StreakText;
    public TMP_Text GameplayText;
    public TMP_Text AttemptsText;
    public Button TypeButton;
    public GameObject HelpScreen;
    private bool helpToggle = false;
    private PlayerPrefUI playerInfo;

    private int vizMode = 1; //0 - Progress, 1 - Stats
    private int numModes = 2;
    private void OnEnable()
    {
        playerInfo = PrefObject.GetComponent<PlayerPrefUI>();
        playerInfo.LoadDataFromPlayerPref(playerInfo.Data.username);
        if (vizMode == 0)
        {
            //Generate Progress vizualization
            TypeButton.GetComponentInChildren<TMP_Text>().text = "Progress";
            Stats.SetActive(false);
            Viz.SetActive(true);
            VizCaptionObject.SetActive(true);
            //VizCaption.enabled = true;
            int[] values = { 10, 20, 30, 40, 50, 60, 70 };
            Slider[] vizSlides = Viz.GetComponentsInChildren<Slider>();

            ArrayList arrDailyPlay = playerInfo.Data.GetAllDailyPlayMins();
            ArrayList arrDates = playerInfo.Data.GetDailyPlayDates();
            Dictionary<string, int> dayVizDict = new Dictionary<string, int>
            {
                ["Sun"] = 0,
                ["Mon"] = 1,
                ["Tue"] = 2,
                ["Wed"] = 3,
                ["Thu"] = 4,
                ["Fri"] = 5,
                ["Sat"] = 6
            };
            int playCount = arrDailyPlay.Count;
            double hold;
            float calVal;
            DateTime datePlay;
            string dayStr;
            double dailyLimit = 10; //i.e. 10 mins per day.

            for (int i = 0; i <playCount; i++)
            {
               hold = (double)arrDailyPlay[i]/dailyLimit * 100;
               calVal = (float)hold;

               datePlay = DateTime.Parse((string)arrDates[i]);
               dayStr = datePlay.DayOfWeek.ToString().Substring(0,3);
               vizSlides[dayVizDict[dayStr]].value = calVal;
            }
            
            /*
            for (int i = 0; i < vizSlides.Length; i++)
            {
                if (i < playCount)
                {

                    double hold = (double)arrDailyPlay[i] * 10;
                    float calVal =(float)hold;
                    vizSlides[i].value =calVal;
                    //vizSlides[i].value = values[i];
                    DateTime datePlay = DateTime.Parse((string)arrDates[i]);
                    string dayStr = datePlay.DayOfWeek.ToString().Substring(3);

                }

            }*/
        }
        else
        {
            TypeButton.GetComponentInChildren<TMP_Text>().text = "Stats";
            Viz.SetActive(false);
            VizCaptionObject.SetActive(false);
            //VizCaption.enabled = false;
            Stats.SetActive(true);
            //Test values. to load actual values next time..
            StatsDate.text = System.DateTime.Now.ToString();
            StreakText.text = playerInfo.Data.streak.ToString();
            GameplayText.text = playerInfo.Data.GetDailyPlayMins().ToString();
            AttemptsText.text = playerInfo.Data.GetDailyAttemptsCount().ToString();

        }
        
    }

    public void ToHelp()
    {
        HelpScreen.SetActive(!helpToggle);
        helpToggle = !helpToggle;
    }
    public void togglePrevMode()
    {
        vizMode += 1;
        vizMode %= numModes;
        OnEnable();
    }

    public void toggleNextMode()
    {
        vizMode -= 1;
        vizMode %= numModes;
        OnEnable();
    }


}

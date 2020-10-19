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
            for (int i = 0; i < vizSlides.Length; i++)
            {
                vizSlides[i].value = values[i];
            }
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

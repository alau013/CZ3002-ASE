using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class DashboardScript : MonoBehaviour
{
    public GameObject MainScreen;
    public GameObject DashboardScreen;
    public GameObject Viz;
    public TMP_Text VizCaption;
    public Button TypeButton;

    private int vizMode = 0; //0 - Progress, 1-?, 2-?
    private int numModes = 2;
    private void Awake()
    {
        if (vizMode == 0)
        {
            //Generate Progress vizualization
            TypeButton.GetComponentInChildren<TMP_Text>().text = "Progress";
            Viz.SetActive(true);
            VizCaption.enabled = true;
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
            VizCaption.enabled = false;
        }
        
    }

    public void togglePrevMode()
    {
        vizMode += 1;
        vizMode %= numModes;
        Awake();
    }

    public void toggleNextMode()
    {
        vizMode -= 1;
        vizMode %= numModes;
        Awake();
    }


}

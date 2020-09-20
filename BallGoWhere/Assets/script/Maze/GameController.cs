using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public bool gamePlaying {get; private set;}
    public GameObject hudContainer;
    public Text timeCounter;

    private float startTime, elapsedTime;
    TimeSpan timePlaying;


    private void Awake()
    {
        instance = this;
    }
    private void start()
    {
        gamePlaying = false;

        //BeginGame();
    }

    public void EndGame()
    {
        gamePlaying = false;
    }

    public void BeginGame()
    {
        gamePlaying = true;
        
       timeController.instance.BeginTimer();
    }

    private void update()
    {
    
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeController : MonoBehaviour
{
   public static timeController instance;

   public Text timeCounter;

   // system namespace for formatting time
   private TimeSpan timePlaying;
   private bool timerGoing;

   private float elapsedTime;

   private void Awake() 
   {
       instance = this;
   }

   private void Start() 
   {
       timeCounter.text = "Time: 00:00.00";
       timerGoing = false;

   }

   public void BeginTimer()
   {
       timerGoing = true;
       elapsedTime = 0f;

       StartCoroutine(UpdateTimer());
   }

   public void EndTimer()
   {
       timerGoing = false;
   }

   private IEnumerator UpdateTimer()
   {
       while (timerGoing)
       {
           elapsedTime += Time.deltaTime;
           //Debug.Log("Countdown: " + elapsedTime.ToString("f0"));
           timePlaying = TimeSpan.FromSeconds(elapsedTime);
           string timePlayingStr = "Time: "+ timePlaying.ToString("mm':'ss'.'ff");
           timeCounter.text = timePlayingStr;

           yield return null;
       }
   }

   public int GetPlayTime()
   {
       elapsedTime += Time.deltaTime;
       return Mathf.FloorToInt(elapsedTime);  
        
   }

}

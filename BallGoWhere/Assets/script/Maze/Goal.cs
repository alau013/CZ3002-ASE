using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject WinPanel;
    public timeController TimeController;

     private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
        Color randomlySelectedColor = GetRandomColorWithAlpha();
        GetComponent<Renderer>().material.color = randomlySelectedColor;
        
        Time.timeScale = 0f;
        WinPanel.SetActive(true);
        WinPanel.transform.Find("scoreText").GetComponent<Text>().text = "your score: "+ (TimeController.GetPlayTime()*10).ToString();

        }
    }

    private Color GetRandomColorWithAlpha()
    {
        return new Color(
            r:UnityEngine.Random.Range(0f,1f),
            g:UnityEngine.Random.Range(0f,1f),
            b:UnityEngine.Random.Range(0f,1f));
        
    }
}

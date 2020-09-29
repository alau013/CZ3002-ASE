using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
        Color randomlySelectedColor = GetRandomColorWithAlpha();
        GetComponent<Renderer>().material.color = randomlySelectedColor;
        Time.timeScale = 0f;
            Debug.Log("test");
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

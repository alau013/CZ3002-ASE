using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelHandle : MonoBehaviour
{
    // Start is called before the first frame update
    public string type;
    public int levelInfo;

    public bool isLast;

    public int getLevelInfo()
    {
        return levelInfo;
    }
        
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gyroManager : MonoBehaviour
{
    #region Instance
    private static gyroManager instance;
    public static gyroManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<gyroManager>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned gyroManager",typeof(gyroManager)).GetComponent<gyroManager>();
                }
            }

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion

    [Header("Logic")]
    private Gyroscope gyro;
    private Quaternion rotation;
    private bool gyroActive;

    public void EnableGyro()
    {
        if (gyroActive)
            return;

        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
             gyroActive = gyro.enabled;
        }


    }

    private void Update()
    {
        if (gyroActive)
        {
            rotation = gyro.attitude;
        }
    }

    public Quaternion GetGryoRotation()
    {
        return rotation;
    }

   

}

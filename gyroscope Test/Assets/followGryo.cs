using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followGryo : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Tweaks")]
    [SerializeField] private Quaternion baseRotation = new Quaternion(0,0,1,0);
    private void Start()
    {
        gyroManager.Instance.EnableGyro();
    }

    // Update is called once per frame
    private void Update()
    {
       transform.localRotation = gyroManager.Instance.GetGryoRotation() * baseRotation; 
    }
}

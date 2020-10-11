using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballcontrol : MonoBehaviour
{
   // Move object using accelerometer
    float speed = 1.0f;

    // Update is called once per frame
    private Rigidbody rigid;

    private void Start() 
    {
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate(){

        if(GameController.instance.gamePlaying == true)
        {
            Vector3 movement = new Vector3 (Input.acceleration.x, 0.0f, Input.acceleration.y);
        
        // old alternative method
        // rigid.velocity = movement * speed;

        // clamp acceleration vector to unit sphere
        if (movement.sqrMagnitude > 1)
            movement.Normalize();
        rigid.AddForce(movement*speed,ForceMode.VelocityChange);
       // Debug.DrawRay(transform.position + Vector3.up, movement, Color.cyan);
        }
     }
}

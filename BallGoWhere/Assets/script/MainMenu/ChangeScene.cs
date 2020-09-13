using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// class method for changing scene
//  https://youtu.be/Gywxwvsb5CA
public class ChangeScene : MonoBehaviour
{
   public void btn_change_scene(string scene_name)
   {
       SceneManager.LoadScene(scene_name);
   }
}

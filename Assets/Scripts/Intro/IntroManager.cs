using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class IntroManager : MonoBehaviour
{
    public Button startButton;

   public void OnStart() 
    {
        SceneManager.LoadScene("Ingame Scene");   
    }

}

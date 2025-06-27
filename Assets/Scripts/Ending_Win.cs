using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Ending_Win : MonoBehaviour
{
    public Button retryBtn; //게임 재시작
    public Button gameoverBtn; //게임종료

    //게임 재시작
    public void retry()
    {
        SceneManager.LoadScene(1);
    }

    //게임종료
    public void GameOver()
    {
        Application.Quit();
    }

    public void Playtime()
    {

    }
}

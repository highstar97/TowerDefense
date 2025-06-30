using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Button retryBtn; //게임 재시작
    public Button gameoverBtn; //게임종료
    public Text playTime; //게임 시간
    public Text CoinCount; //코인 얻기

    //게임 재시작 - 하정우
    public void retry()
    {
        SceneManager.LoadScene(1);
    }

    //게임종료 - 하정우
    public void GameOver()
    {
        Application.Quit();
    }

    // Ending UI 점수표기 - 하정우
    void Start()
    {
        CoinCount.text = "생존 시간 : " + GameObject.Find("CoinManager").GetComponent<CoinManager>().totalEarnedCoin.ToString();
        PlayTimeController timeController = GameObject.Find("TimeController").GetComponent<PlayTimeController>();

        playTime.text = "획득 코인 : " + (timeController.initTime - timeController.remainedTime).ToString();
    }
}
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

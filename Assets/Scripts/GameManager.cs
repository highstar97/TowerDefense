using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isPlayerWin = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ContextMenu(nameof(Gameover))]
    private void Gameover()
    {
        SceneManager.LoadScene(3);  // 패배 신
    }


    public void GameOver()
    {
        if(isPlayerWin)
        {
            SceneManager.LoadScene(2);  // 승리 신
        }
        else
        {
            SceneManager.LoadScene(3);  // 패배 신
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUI : MonoBehaviour
{
    public Text playTimeText;
    public Text coinText;

  
    public void OnChangedPlayTime(float t) //게임 타이머 
    {
        playTimeText.text = $"{(int)(t / 60):00}:{(int)(t % 60):00}";
    }
    public void OnCoin(int maxCoin, int currentCoin) //코인
    {
        coinText.text = $"{currentCoin} / {maxCoin}";
    }


}

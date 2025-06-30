using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUI : MonoBehaviour
{
    public Text playTimeText;
    public Text coinText;
  
    /*작성자 : 임은성, 기능 : 게임타이머 ,코인 UI 표시 */
    public void UpdatePlayTimeUI(float t) 
    {
        playTimeText.text = $"{(int)(t / 60):00}:{(int)(t % 60):00}";
    }
    public void UpdateConiUI(int currentCoin) 
    {
        coinText.text = $"{currentCoin}";
    }


}

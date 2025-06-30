using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance; //코인 싱글톤 객체생성

    public int totalEarnedCoin = 0;

    private int currentCoin = 0;

    public int CurrentCoin { get { return currentCoin; } }

    [SerializeField]
    private CanvasUI canvasUI;  // CanvasUI 에 연결

    [SerializeField]
    private GameObject towerPrefab; //타워 프리팹 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        UpdateUI();
    }

    [ContextMenu(nameof(AddCoin))]
    private void AddCoin()
    {
        // Hack: 코인 디버깅용
        currentCoin += 100;
        UpdateUI();
    }


    /* 작성자 : 임은성
     * 기능: 코인 획득함수 
     **/

    public void AddCoin(int amount)
    {
        totalEarnedCoin += amount;
        currentCoin += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (canvasUI != null)
        {
            canvasUI.UpdateConiUI(currentCoin);
        }
    }

    public void UseCoin(int amount)
    {
        // 현재 코인 보유량보다 높은면 return
        if (currentCoin < amount) return;
        currentCoin -= amount;
        Debug.Log(amount + "비용");
        Debug.Log(currentCoin + "원");
        UpdateUI();
    }

}



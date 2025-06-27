using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance; //코인 싱글톤 객체생성

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

    public void AddCoin(int amount)
    {
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

        UpdateUI();
    }

}



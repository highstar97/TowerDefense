using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int maxCoin = 100;
    private int currentCoin = 0;

    [SerializeField]
    private CanvasUI canvasUI;  // CanvasUI 컴포넌트를 에디터에서 연결

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // 초기 UI 갱신
        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        currentCoin += amount;
        if (currentCoin > maxCoin)
            currentCoin = maxCoin;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (canvasUI != null)
        {
            canvasUI.OnCoin(maxCoin, currentCoin);
            if (currentCoin >= maxCoin)
            {
              //코인 모으면 타워 생성
            }
        }
    }

    
}

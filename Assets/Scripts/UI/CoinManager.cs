using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance; //코인 싱글톤 객체생성

    public int maxCoin = 100;
    private int currentCoin = 0;

    public Image addTowerImage;

    [SerializeField]
    private CanvasUI canvasUI;  // CanvasUI 에 연결

    [SerializeField]
    private GameObject towerPrefab; //타워 프리팹 

    void Start()
    {
        if (addTowerImage != null)
        {
            addTowerImage.gameObject.SetActive(false);
        }
    }


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
        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        currentCoin += amount;
        if (currentCoin > maxCoin)
        {
            currentCoin = maxCoin;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (canvasUI != null)
        {
            canvasUI.OnCoin(maxCoin, currentCoin);
        }
        if (addTowerImage != null)
        {
            addTowerImage.gameObject.SetActive(currentCoin >= maxCoin);
        }
    }

    public void AddTower()
    {
        if (currentCoin >= maxCoin)
        {
            Vector3 spawnPosition = new Vector3(-22.69f, -0.13f, -1.46f);
            GameObject tower = Instantiate(towerPrefab, spawnPosition, Quaternion.identity);

            currentCoin = 0;  // 코인 리셋
            UpdateUI();       // UI 
        }
    }
}
    


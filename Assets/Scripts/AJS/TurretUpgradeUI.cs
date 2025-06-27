using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretUpgradeUI : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private TurretAttack turretAttack; // Turrer Prefab에 있는 TurretAttack

    [Header("Buttons")]
    public Button speedUpgrade;
    private TextMeshProUGUI speedUpgradeCostText;

    public Button rangeUpgrade;
    private TextMeshProUGUI rangeUgradeCostText;

    public Button damageUpgrade;
    private TextMeshProUGUI damageUpgradeCostText;

    [Header("Stat Text")]
    public TextMeshProUGUI speedStat;

    public TextMeshProUGUI rangeStat;

    public TextMeshProUGUI damageStat;

    [Header("Upgrade Cost")]
    [SerializeField]
    private int speedUpgradeCost; // 스피드 업그레이드 비용

    [SerializeField]
    private int rangeUpgradeCost; // 범위 업그레이드 비용

    [SerializeField]
    private int damageUpgradeCost; // 데미지 업그레이드 비용

    #endregion

    #region Unity Functions;

    private void Start()
    {
        speedUpgrade.onClick.AddListener(UpgradeSpeedOnClick);
        rangeUpgrade.onClick.AddListener(UpgradeRangeOnClick);
        damageUpgrade.onClick.AddListener(UpgradeDamageOnClick);

        speedUpgradeCostText = speedUpgrade.GetComponentInChildren<TextMeshProUGUI>();
        rangeUgradeCostText = rangeUpgrade.GetComponentInChildren<TextMeshProUGUI>();
        damageUpgradeCostText = damageUpgrade.GetComponentInChildren<TextMeshProUGUI>();

        // State UI 초기화
        damageStat.text = "Speed : " + turretAttack.AttackDamage.ToString();
        rangeStat.text = "Range : " + turretAttack.AttackRange.ToString();
        speedStat.text = "Speed : " + turretAttack.AttackSpeed.ToString();

        damageUpgradeCostText.text = damageUpgradeCost + " Coin";
        rangeUgradeCostText.text = rangeUpgradeCost + " Coin";
        speedUpgradeCostText.text = speedUpgradeCost + " Coin";
    }

    #endregion

    #region User Functions

    public void UpgradeDamageOnClick()
    {
        // 업그레이드 비용이 충분하면
        if(CoinManager.Instance.CurrentCoin > damageUpgradeCost)
        {
            Debug.Log(damageUpgradeCost);
            // 업그레이드 비용만큼 비용 감소
            CoinManager.Instance.UseCoin(damageUpgradeCost);
            // 현재는 데미지 1 상승
            turretAttack.UpgradeDamage(1);
            // 다음 업그레이드 비용 얻기
            damageUpgradeCost = GetNextUpgradeCost(damageUpgradeCost);
            // 현재 스탯 UI 업데이트
            damageStat.text = "Damage : " + turretAttack.AttackDamage.ToString();
            damageUpgradeCostText.text = damageUpgradeCost + " Coin";
        }
    }
    public void UpgradeRangeOnClick()
    {
        // 업그레이드 비용이 충분하면
        if (CoinManager.Instance.CurrentCoin > rangeUpgradeCost)
        {
            Debug.Log(rangeUpgradeCost);
            // 업그레이드 비용만큼 비용 감소
            CoinManager.Instance.UseCoin(rangeUpgradeCost);
            // 현재는 1 상승
            turretAttack.UpgradeRange(1);
            // 다음 업그레이드 비용 얻기
            rangeUpgradeCost = GetNextUpgradeCost(rangeUpgradeCost);
            // 현재 스탯 UI 업데이트
            rangeStat.text = "Range : " + turretAttack.AttackRange.ToString();
            rangeUgradeCostText.text = rangeUpgradeCost + " Coin";
        }
    }
    public void UpgradeSpeedOnClick()
    {
        // 업그레이드 비용이 충분하면
        if (CoinManager.Instance.CurrentCoin > speedUpgradeCost)
        {
            Debug.Log(speedUpgradeCost);
            // 업그레이드 비용만큼 비용 감소
            CoinManager.Instance.UseCoin(speedUpgradeCost);
            // 현재는 1 상승
            turretAttack.UpgradeSpeed(1);
            // 다음 업그레이드 비용 얻기
            speedUpgradeCost = GetNextUpgradeCost(speedUpgradeCost);
            // 현재 스탯 UI 업데이트
            speedStat.text = "Speed : " + turretAttack.AttackSpeed.ToString();
            speedUpgradeCostText.text = speedUpgradeCost + " Coin";
        }
    }

    private int GetNextUpgradeCost(int currentCost)
    {
        int newCost;

        newCost= currentCost+2;
        return newCost;
    }
    #endregion
}

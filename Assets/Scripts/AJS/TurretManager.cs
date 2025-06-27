using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject buildUI; // 터렛 건설하는 UI

    [SerializeField]
    private GameObject upgradeUI;// 터렛 업그레이드 하는 UI

    [SerializeField]
    private GameObject turretPrefab;

    [SerializeField]
    private int turretCost; // 터렛 구매 비용

    #endregion

    #region Unity Functions;

    private void Start()
    {
        buildUI.SetActive(true);
        // 시작시 터렛 비활성화
        turretPrefab.SetActive(false);
    }
    #endregion

    #region User Functions

    // BuildUI에 있는 구매 버튼 OnClick에 넣을 함수
    public void BuildTurret()
    {
        //CoinManager.Instance.
        if(CoinManager.Instance.CurrentCoin >= turretCost)
        {
            // 터렛 비용만큼 코인 감소
            CoinManager.Instance.UseCoin(turretCost);

            // 터렛 활성화
            turretPrefab.SetActive(true); 
        }
    }

    #endregion
}

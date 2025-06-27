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
        //시작시 Build UI만 활성화
        buildUI.SetActive(true);
        turretPrefab.SetActive(false);
        upgradeUI.SetActive(false);  
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
            // Upgrade UI 활성화
            upgradeUI.SetActive(true);
            // Build UI 비활성화
            buildUI.SetActive(false);
        }
        else
        {
            // Hack: TurreTManger 디버그용
            // Todo: UI Text로 변경되게 수정 예정 / 비용도 같이 표시
            Debug.Log("돈 부족함");
        }
    }

    #endregion
}

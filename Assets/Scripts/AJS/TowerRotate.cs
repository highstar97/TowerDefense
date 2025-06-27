using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerAttack))]
public class TowerRotate : MonoBehaviour
{
    #region Variables

    private TowerAttack towerAttack; // TargetEnemy 값 가져오기

    public float rotationSpeed = 5f; // 회전 속도

    #endregion

    #region Unity Functions;

    private void Start()
    {
        towerAttack = GetComponent<TowerAttack>();
    }

    private void Update()
    {
        if (towerAttack.TargetEnemy != null)
        {
            // 타겟을 향한 방향 벡터 계산
            Vector3 directionToTarget = towerAttack.TargetEnemy.gameObject.transform.position - transform.position;
            directionToTarget.y = 0; // Y축 회전을 막아 수평으로만 바라보도록 설정 (필요에 따라 조절)

            // 부드럽게 회전
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    #endregion

}

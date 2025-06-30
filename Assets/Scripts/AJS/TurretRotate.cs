using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretAttack))]
public class TurretRotate : MonoBehaviour
{
    #region Variables

    private TurretAttack towerAttack; // TargetEnemy 값 가져오기

    public float rotationSpeed = 5f; // 회전 속도

    #endregion

    #region Unity Functions;

    private void Start()
    {
        towerAttack = GetComponent<TurretAttack>();
    }

    // 작성자: 안진성, 기능: 범위 안에 있는 적을 바라보는 함수
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

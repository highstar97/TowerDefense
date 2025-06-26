using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class TowerAttack : MonoBehaviour
{
    #region Variables
    // === 체력 스탯 변수 ===

    [SerializeField]
    private int maxHp = 3;

    private int currentHp;

    // === 공격 스탯 변수 ===
    [SerializeField]
    private int attackDamage = 1;                    // 공격력

    [SerializeField]
    private float attackRange = 3f;                  // 공격 범위

    [SerializeField]
    private float attackSpeed = 2f;                  // 공격 속도

    public LayerMask targetLayerMasks;               // Enemy LayerMask

    // === 공격 기능 변수 ===

    private bool isAttacking = false;                // 공격 중인지 여부

    private Enemy targetEnemy;                       // 현재 공격 중인 적

    // Hack: TriggerEnter 사용을 위한, EnemyPrefab에 RigidBody 추가
    // Hack: Enemy 공격 받으면 멈춰서 어색함

    private void OnValidate()
    {
        if (targetLayerMasks.value == 0) // TargerLayer가 nothing이면
        {
            targetLayerMasks = LayerMask.GetMask("Enemy"); // Enemy Layer로 설정
        }
    }

    private void Start()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = attackRange; // 공격 범위 동기화

        currentHp = maxHp; // 체력 초기화
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking || targetEnemy != null)
        {
            return; // 이미 공격하는 적이 있으면, 추가 공격 방지
        }

        // Enemy 인지 확인
        if ((targetLayerMasks.value & (1 << other.gameObject.layer)) != 0)
        {

            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                StopAllCoroutines(); // 실행 중인 공격 정지
                targetEnemy = enemy;    // 현재 공격 중인 타켓 수정
                StartCoroutine(AttackTargetEnemy());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 나간 적인 Enemy 인지 확인
        if ((targetLayerMasks.value & (1 << other.gameObject.layer)) != 0)
        {
            // 공격 중인 Enemy가, 방금 나간 적인지 확인
            if (targetEnemy != null && targetEnemy.gameObject == other.gameObject)
            {
                StopAllCoroutines(); // 공격 멈추고
                targetEnemy = null;  // Target null 설정
                isAttacking = false; // 공격상태 false로
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        // 공격 범위 기즈모로 시각화
        if (isAttacking)
        {
            // 공격 중이면 빨간색
            Gizmos.color = Color.red;
        }
        else
        {
            // 아니면 초록색으로 표시
            Gizmos.color = Color.green;
        }
        
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    #endregion

    #region User Functions

    private IEnumerator AttackTargetEnemy()
    {
        isAttacking = true; // 공격 상태 활성화
        while (targetEnemy != null)
        {
            targetEnemy.TakeDamage(attackDamage); // 공격력 만큼 데미지 주기
            yield return new WaitForSeconds(attackSpeed); // 공격 속도 만큼 대기 후 반복
        }

        isAttacking = false; // 공격 상태 비활성화
    }
    #endregion

}

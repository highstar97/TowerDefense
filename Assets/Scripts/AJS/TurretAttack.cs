using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class TurretAttack : MonoBehaviour
{
    #region Variables

    public bool gizmosActive = true; // Hack : 기즈모 활성화 여부, 제거 가능

    // === 공격 스탯 변수 ===

    [Header("Attack")]
    [SerializeField]
    private int attackDamage = 1;                    // 공격력
    public int AttackDamage { get { return attackDamage; } }

    [SerializeField]
    private float attackRange = 5f;                  // 공격 범위
    public float AttackRange { get { return attackRange; } }

    private SphereCollider rangeColliders;

    [SerializeField]
    private float attackSpeed = 5f;                  // 공격 속도
    public float AttackSpeed { get { return attackSpeed; } }

    public Transform hitPos;                        // 공격 발사 지점

    // === 공격 기능 변수 ===

    [Header("Target")]
    public LayerMask targetLayerMasks;               // Enemy LayerMask

    public LayerMask allianceLayerMasks;             // alliance Layer Masks;

    private Enemy targetEnemy;                       // 현재 공격 중인 적
    public Enemy TargetEnemy
    {
        get { return targetEnemy; }
    }

    private List<Enemy> targetLists = new List<Enemy>();  // 현재 범위 안에 있는 적 리스트

    private bool isAttacking = false;                // 공격 중인지 여부

    private EffectSpawner effectSpawner;             // Bullet Effect Spawner

    private Animator animator;

    #endregion

    #region Unity Functions;

    // Hack: Enemy 공격 받으면 멈춰서 어색함

    private void OnValidate()
    {
        if (targetLayerMasks.value == 0) // TargerLayer가 nothing이면
        {
            targetLayerMasks = LayerMask.GetMask("Enemy"); // Enemy Layer로 설정
        }
        if (allianceLayerMasks.value == 0) // allianceLayerMasks가  nothing이면
        {
            allianceLayerMasks = LayerMask.GetMask("Player"); //Player Layer로 설정
        }
        if (hitPos == null)
        {
            Debug.LogError("Tower의 hitPos이 null 상태 입니다!");
        }
    }

    // 작성자: 안진성, 기능: 공격 스탯 및 effectSpawner 초기화
    private void Start()
    {
        effectSpawner = GameObject.Find("Bullet Effect Spawner").GetComponent<EffectSpawner>();

        rangeColliders = GetComponent<SphereCollider>();
        rangeColliders.radius = attackRange; // 공격 범위 동기화

        animator = GetComponent<Animator>(); // animator 컴포넌트 참조
    }

    // 작성자: 안진성, 기능: 거리 안에 들어온 적 리스트로 관리
    private void OnTriggerEnter(Collider other)
    {
        // Enemy 인지 확인
        if ((targetLayerMasks.value & (1 << other.gameObject.layer)) != 0)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // 리스트에 이미 없는 경우에만 추가하여 중복 방지
            if (!targetLists.Contains(enemy))
            {
                // 리스트에 해당 적 추가
                targetLists.Add(enemy);
            }

            // 현재 공격 중이 아니고, 타켓이 없으면
            if (!isAttacking && targetEnemy == null)
            {
                // 새로운 타켓 설정
                FindNewTarget();
            }
        }
    }
    // 작성자: 안진성, 기능: 거리 밖에 나간 적 관리 및 현재 타켓인지 확인
    private void OnTriggerExit(Collider other)
    {
        // 나간 적인 Enemy 인지 확인
        if ((targetLayerMasks.value & (1 << other.gameObject.layer)) != 0)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // 타켓이, 방금 나간 적인지 확인
            if (targetEnemy == enemy)
            {
                StopAllCoroutines(); // 코루틴 정지

                // 공격상태 비활성화
                targetEnemy = null;  
                isAttacking = false;

                // 리스트에 해당 적 제외
                targetLists.Remove(enemy);

                // 새로운 적 탐색
                FindNewTarget();
            }
            else
            {
                // 리스트에 해당 적 제외
                targetLists.Remove(enemy);
            }
        }
    }
    // 작성자: 안진성, 기능: 공격 범위 시각화
    private void OnDrawGizmos()
    {
        if (!gizmosActive) return; // 기즈모 활성화 여부

        // 공격 범위 기즈모로 시각화
        if (isAttacking)
        {
            // 공격 중이면 빨간색
            Gizmos.color = Color.red;
        }
        else
        {
            // 아니면 초록색으로 표시
            Gizmos.color = Color.blue;
        }

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    #endregion

    #region User Functions

    // 작성자: 안진성, 기능: 공격 속도 만큼 공격 반복 및 이펙트
    private IEnumerator AttackTargetEnemy()
    {
        isAttacking = true; // 공격 상태 활성화    

        // Hack: null로 타켓의 죽음을 인식해서 딜레이가 생길 수 있음/ 이벤트로 인식하는 방식으로 수정 가능
        while (targetEnemy != null)
        {
            // hitPos과 타켓의 사이의 방향
            Vector3 directionToTarget = targetEnemy.transform.position - hitPos.position;
            directionToTarget.y = 0;

            // hitPos에서  이펙트 생성
            effectSpawner.SpawnEffect(hitPos.position, directionToTarget);

            animator.SetTrigger("Fire");

            // hitPos에서 Ray 발사 후, Enemy 맞으면
            Ray ray = new Ray(hitPos.position, directionToTarget);
            RaycastHit hitResult;
            if (Physics.Raycast(ray, out hitResult, 200, ~allianceLayerMasks))
            {
                // Effect Spawner에서 생성
                effectSpawner.SpawnEffect(hitResult.point, hitResult.normal);
            }

            // 공격력 만큼 데미지 주기
            targetEnemy.TakeDamage(attackDamage);
            // 공격 속도 만큼 대기 후 반복
            yield return new WaitForSeconds(attackSpeed);
        }
        // 타켓이 죽으면 새로운 타켓 찾기
        FindNewTarget();
    }


    /// <summary>
    ///  공격 범위 내 적 리스트에서 새로운 대상을 선택합니다.
    /// </summary>
    /// <remarks>이 메서드는 적 리스트에서 null 항목을 제거하고,<br/> 첫 번째 적을 새로운 대상으로 선택합니다.<br/>
    /// 만약 사용 가능한 적이 없다면, 공격 상태가 비활성화됩니다.</remarks>
    private void FindNewTarget()
    {
        // 제거되서 null 상태인 적들 리스트에서 정리
        targetLists.RemoveAll(enemy => enemy == null);

        // 리스트에 적이 있으면
        if (targetLists.Count > 0)
        {
            // 실행 중인 공격 코루틴 정지
            StopAllCoroutines();

            // 리스트에 첫번째 적을 타켓으로 설정
            targetEnemy = targetLists[0];
            // 공격 코루틴 실행
            StartCoroutine(AttackTargetEnemy());
        }
        else
        {
            // 적이 없으면 공격 상태 비활성화
            targetEnemy = null;  
            isAttacking = false;
        }
    }

    // TurretUpgradeUI에서 호출할 함수들
    public void UpgradeDamage(int amount)
    {
        attackDamage += amount;
    }

    public void UpgradeRange(int amount)
    {
        attackRange += amount;
        rangeColliders.radius = attackRange; // 공격 범위 동기화
    }

    public void UpgradeSpeed(int amount)
    {
        attackSpeed -= amount;
        if(attackSpeed < 0) attackSpeed = 0.1f;
    }
    #endregion

}

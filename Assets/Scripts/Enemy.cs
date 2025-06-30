using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ITakeDamageable
{
    #region Variables
    enum EnemyState     // State of enemy
    {
        Idle, Move, Attack, Damage, Die
    }

    private EnemyState currentState = EnemyState.Idle;  // current state

    [SerializeField]
    private int maxHp = 3;                      // max hp

    private int currentHp;                      // current hp

    public int currentCoin;

    public int maxCoint = 100;

    [SerializeField]
    private float durationOfIdleState = 2f;     // duration of idle state

    [SerializeField]
    private float moveSpeed = 1f;               // speed of moving

    [SerializeField]
    private float attackRange = 5f;             // range of attack

    [SerializeField]
    private float attackSpeed = 0.5f;           // speed of attack

    private float elapsedTime = 0f;             // elapsed time

    [SerializeField]
    private GameObject healthPointPrefab;       // Health Point Prefab

    private List<GameObject> healthPoints = new ();      // list of Health Points

    private Transform healthUITransform;        // health UI Transform;

    private Transform targetTransform;          // target Transform

    private Material material;                  // Varialbe of Mesh Renderer Component

    private Animator animator;                  // animator

    private NavMeshAgent navMeshAgent;          // Nav Mesh Agent Component

    [SerializeField]
    private GameObject coinPrefab; // 코인 프리팹
    
    private LineRenderer lineRenderer;          // Line Renderer

    private EffectSpawner effectSpawner;        // Bomb Effect Spawner
    #endregion

    #region Unity Functions
    private void Awake()
    {
        animator = GetComponent<Animator>();

        lineRenderer = GetComponent<LineRenderer>();    // Line Renderer 찾기
        lineRenderer.positionCount = 2;                 // 사용할 점을 2개로 변경
        lineRenderer.enabled = false;                   // line Renderer 비활성화

        effectSpawner = GameObject.Find("Explosion Effect Spawner").GetComponent<EffectSpawner>();
    }

    private void Start()
    {
        currentHp = maxHp;

        healthUITransform = transform.Find("Health UI").transform;
        for (int i = 0; i < currentHp; ++i)
        {
            healthPoints.Add(Instantiate(healthPointPrefab, healthUITransform));
        }

        targetTransform = GameObject.Find("Target Position").transform;

        material = GetComponentInChildren<MeshRenderer>().material;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;
        navMeshAgent.speed = moveSpeed;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damage:
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }
    #endregion

    #region User Functions
    // 작성자 : 박규탁
    // 기  능 : OnDamageProcess()를 대신해서 사용하는 데미지 처리 함수. ITakeDamageable 인터페이스로 상속된 함수
    //         받은 데미지 중 처리할 수 있는 데미지만큼 UI 처리
    public void TakeDamage(int damageAmount = 1)
    {
        int availableDamage = Mathf.Min(currentHp, damageAmount);
        
        currentHp -= availableDamage;
        for (int i = 0; i < availableDamage; ++i)
        {
            Destroy(healthPoints[0]);
            healthPoints.RemoveAt(0);
        }

        if (currentHp > 0)
        {
            currentState = EnemyState.Damage;
            StopAllCoroutines();
            StartCoroutine(Damage());
        }
        else
        {
            StopAllCoroutines();
            currentState = EnemyState.Die;
        }
    }

    // 작성자 : 강사님
    private void Idle()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > durationOfIdleState)
        {
            currentState = EnemyState.Move;
        }
    }

    // 작성자 : 강사님
    private void Move()
    {
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(targetTransform.position);

        if (Vector3.Distance(this.transform.position, targetTransform.position) < attackRange)
        {
            currentState = EnemyState.Attack;
            navMeshAgent.enabled = false;
        }
    }

    // 작성자 : 박규탁
    // 기  능 : 공격 속도에 비례해서 공격을 진행하는 함수. 실행 시 애니메이션 같이 실행
    private void Attack()
    {
        elapsedTime += Time.deltaTime;
        float attackDelayTime = 1.0f / attackSpeed;

        if (elapsedTime > attackDelayTime)
        {
            this.transform.LookAt(targetTransform);

            animator.SetTrigger("Attack");
            Player.Instance.TakeDamage(1);
            elapsedTime = 0.0f;
        }
    }
    
    // 작성자 : 강사님
    private IEnumerator Damage()
    {
        navMeshAgent.enabled = false;   // Stop Finding Paths

        Color originalColor = material.color;
        material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        material.color = originalColor;

        currentState = EnemyState.Idle;
        elapsedTime = 0f;
    }

    private void Die()
    {
        effectSpawner.SpawnEffect(this.transform.position, this.transform.rotation.eulerAngles);

        Vector3 coinSpawnPosition = transform.position + Vector3.up * 0.5f;

        Instantiate(coinPrefab, coinSpawnPosition, Quaternion.Euler(-90f,0f,0f));

        Destroy(this.gameObject);
    }

    // 작성자 : 박규탁
    // 기  능 : Attack 실행 시, 라인렌더러를 통해서 공격하는 것을 시각화 해주는 함수(Draw Line Effect)코루틴을 실행
    public void ActiveLineEffect()
    {
        StartCoroutine(DrawLineEffect(this.transform.position, targetTransform.position, 0.1f));
    }

    // 작성자 : 박규탁
    // 기  능 : 라인렌더러를 활성화하여 탄알 궤적 그리는 함수
    private IEnumerator DrawLineEffect(Vector3 startPosition, Vector3 endPosition, float time)
    {
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);

        lineRenderer.enabled = true;        // 라인 렌더러를 활성화하여 탄알 궤적을 그림

        yield return new WaitForSeconds(time);

        lineRenderer.enabled = false;       // 라인 렌더러를 비활성화하여 탄알 궤적을 지움
    }
    #endregion
}
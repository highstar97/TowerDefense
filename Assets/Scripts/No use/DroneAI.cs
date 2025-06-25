//using JetBrains.Annotations;
//using Oculus.Interaction.Editor.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    enum DroneState     // State of drones
    {
        Idle,
        Move,
        Attack,
        Damage,
        Die
    }
    DroneState state = DroneState.Idle; //초기 시작 상태는 Idle로 설정
    public float idleDelayTime = 2f; //대기 상태의 지속시간
    float currentTime; //경과 시간

    public float moveSpeed = 1; //공격 속도
    Transform tower; //타워위치(타겟위치)
    NavMeshAgent agent; //내비매쉬 에이전트 컴포넌트
    public float attackRange = 3; //타워와 3미터 거리면 공격 시작
    public float attackDelayTime = 2; //공격 딜레이 시간

    [SerializeField] //private속성 이지만 에디터에 노출이 된다.
    int hp = 3;

    [SerializeField]
    Transform explosion;
    ParticleSystem expEffect;
    AudioSource expAudio;

    void Start()
    {
        tower = GameObject.Find("Tower").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        agent.speed = moveSpeed;

        explosion = GameObject.Find("Explosion").transform;
        expEffect = explosion.GetComponent<ParticleSystem>();
        expAudio = explosion.GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (state)
        {
            case DroneState.Idle:
                Idle();
                break;
            case DroneState.Move:
                Move();
                break;
            case DroneState.Attack:
                Attack();
                break;
            case DroneState.Damage:
                // Damage
                break;
            case DroneState.Die:
                Die();
                break;
        }
    }
    void Idle() 
    {
        currentTime += Time.deltaTime;
        if (currentTime > idleDelayTime)
        {
            state = DroneState.Move;
        }
    }
    void Move()
    {
        agent.enabled = true;
        agent.SetDestination(tower.position);   // 네비게이션의 목적지를 타워로 설정
        if(Vector3.Distance(transform.position, tower.position) < attackRange)
        {
            state = DroneState.Attack;
            agent.enabled = false;
        }
    }
    void Attack()
    {
        currentTime += Time.deltaTime;
        if(currentTime > attackDelayTime)
        {
            // 타워 체력 감소
            --Tower.Instance.HP;
            // 피격 이펙트 효과
            currentTime = 0f;
        }
    }
    public void OnDamageProcess()
    {
        --hp;
        if (hp > 0)
        {
            state = DroneState.Damage;
            StopAllCoroutines();
            StartCoroutine(Damage());
        }
        else
        {
            explosion.position = transform.position;
            expEffect.Play();
            expAudio.Play();
            Destroy(gameObject);
        }
       
    }
    IEnumerator Damage()
    {
        agent.enabled = false;  // 길 찾기 중지
        Material material = GetComponentInChildren<MeshRenderer>().material;
        Color originalColor = material.color;
        material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        material.color = originalColor;
        state = DroneState.Idle;
        currentTime = 0f;
    }
    void Die()
    {

    }
}

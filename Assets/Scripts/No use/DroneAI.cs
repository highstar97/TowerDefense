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
    DroneState state = DroneState.Idle; //�ʱ� ���� ���´� Idle�� ����
    public float idleDelayTime = 2f; //��� ������ ���ӽð�
    float currentTime; //��� �ð�

    public float moveSpeed = 1; //���� �ӵ�
    Transform tower; //Ÿ����ġ(Ÿ����ġ)
    NavMeshAgent agent; //����Ž� ������Ʈ ������Ʈ
    public float attackRange = 3; //Ÿ���� 3���� �Ÿ��� ���� ����
    public float attackDelayTime = 2; //���� ����� �ð�

    [SerializeField] //private�Ӽ� ������ �����Ϳ� ������ �ȴ�.
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
        agent.SetDestination(tower.position);   // �׺���̼��� �������� Ÿ���� ����
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
            // Ÿ�� ü�� ����
            --Tower.Instance.HP;
            // �ǰ� ����Ʈ ȿ��
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
        agent.enabled = false;  // �� ã�� ����
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

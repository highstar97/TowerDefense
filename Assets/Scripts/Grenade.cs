using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("수류탄 잡기")]
    public GameObject grenade; //수류탄 프리팹 가져오기
    public float bombThrowPow = 8f; //던지는 힘
    public Transform holdPosition; //수류탄 잡는 위치
    public Transform throwPoint; // 수류탄 던지는 기준 위치
    private GameObject grabGrenade; // 잡고있는 수류탄(상태 표현용)
    private bool isGrab = false; // 수류탄 잡고있는지 여부.
    private Vector3 Handpos; //던져지는 위치 계산을 위한 변수
    private bool isPause = false; //애니메이션 움직임 on/off;


    [Header("수류탄 정보")]
    public float bombPow = 3f; //수류탄 공격력
    public float bombRange = 5f; // 수류탄 범위
    public LayerMask enemy; //수류탄 맞은 적
    public float explosionTime = 2f; //던지고 터지는 시간

    [Header("기타정보")]
    Animator anim; 
    public GameObject gun;//수류탄 던질때 총 on/off작업
    public GameObject ikScript; //수류탄 던질때 IKscript on/off
    public LineRenderer targetLineRenderer; //궤적 라인
    int linePointer = 40;// 궤적 포인트 갯수(많으면 더 좋음)

    private void Awake()
    {
        targetLineRenderer = GetComponent<LineRenderer>();
        targetLineRenderer.enabled = false;
    }

    //라인렌더러 설정
    void SetuptargetLineRenderer()
    {
        targetLineRenderer.startWidth = 0.1f;
        targetLineRenderer.endWidth = 0.1f;
        targetLineRenderer.startColor = Color.red;
        targetLineRenderer.endColor= Color.red;
        targetLineRenderer.enabled = true; ;
    }

    //궤적 업데이트
    public void UpdateTargetLineRenderer()
    {
        SetuptargetLineRenderer();

        Vector3 velocity = ARAVRInput.RHandDirection * bombThrowPow;
        Vector3 startPos = throwPoint.transform.position;

        targetLineRenderer.positionCount = linePointer;

        for(int i= 0; i<linePointer; i++)
        {
            targetLineRenderer.SetPosition(i, startPos);
            velocity += Physics.gravity * (1f / 15f);
            startPos += velocity * (1f / 15f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 수류탄 잡는 중(수류탄 떨어지는 위치 확인, 던지지는 않음. 오른쪽 버튼 누르고 있는 동안 실행)
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            ReadyGrenade();

        }
        // 오른쪽 버튼을 떼었을때 발생
        if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            ThrowGrenade();
        }

        //라인렌더러 실시간 적용
        if (ARAVRInput.Get(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            UpdateTargetLineRenderer();
        }

    }

    public void ReadyGrenade()
    {
        //수류탄 던지는 모션중에는 총이 사라지고, 총의 IK를 제거함
        anim.SetTrigger("ThrowBomb");
        gun.SetActive(false);
        ikScript.GetComponent<IKScripts>().enabled = false;
    }

    public void pauseAnim()
    {
        
        grabGrenade = Instantiate(grenade, holdPosition.position, holdPosition.rotation);
        grabGrenade.transform.SetParent(holdPosition);
        grabGrenade.GetComponent<Rigidbody>().isKinematic = true;
        isGrab = true;
        Handpos = ARAVRInput.RHandPosition;
        isPause = true;
        anim.speed = 0;
    }

    public void ThrowGrenade()
    {
        isPause = false;
        anim.speed = 1f;
        //던지는 행위 했을때의 물리 적용
        grabGrenade.transform.SetParent(null);
        Rigidbody grenadeRigidbody = grabGrenade.GetComponent<Rigidbody>();
        grenadeRigidbody.isKinematic = false;

        Vector3 throwDirector = ARAVRInput.RHandDirection;
        grenadeRigidbody.velocity = throwDirector * bombThrowPow;

        //터지는 것에 대한 딜레이 적용
        StartCoroutine(ExplosionTime(grabGrenade.transform.position));
        targetLineRenderer.enabled = false;

    }

    IEnumerator ExplosionTime(Vector3 explosionPos)
    {
        yield return new WaitForSeconds(explosionTime);
        Collider[] Enemys = Physics.OverlapSphere(transform.position, bombRange, enemy);
        for(int i = 0; i<Enemys.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, Enemys[i].transform.position);
            //적 배열에서 하나씩 꺼내서 거리 측정함.
            
            float damageDistance = 1 - (distance / bombRange);
            //거리 비교를 위한 식

            int damagedEnemy = Mathf.RoundToInt(bombPow * damageDistance);
            //거리에 따른 계산식
            ITakeDamageable target = Enemys[i].GetComponent<ITakeDamageable>();
            if(target != null)
            {
                target.TakeDamage(damagedEnemy);
            }
        }
    }
    public void Showgun() //총 gameobject on
    {
        gun.SetActive(true);
        ikScript.GetComponent<IKScripts>().enabled = true;
    }
}

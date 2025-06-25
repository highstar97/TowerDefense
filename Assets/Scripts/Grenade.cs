using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float bombPow = 3f; //폭탄 위력
    public float bombRange = 5f; // 폭탄 범위
    public float bombThrowPow = 8f; //던지는 힘
    public LayerMask enemy; //맞힐적 표기
    Animator anim; //수류탄 던지는 모션 설정
    //public GameObject grenade; //수류탄 가져오기

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ThrowGrenade();
    }
    public void ThrowGrenade()
    {
        //마우스 왼쪽버튼을 누르게 되면 수류탄 던지는 모션 발생
        //2초후 터지게 함.
        if(ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            anim.SetTrigger("ThrowBomb");
            StartCoroutine(BombTimer(2f));
        }
    }

    IEnumerator BombTimer(float countdown)
    {
        yield return new WaitForSeconds(countdown);
        Collider[] Enemys = Physics.OverlapSphere(transform.position, bombRange, enemy);
        for(int i = 0; i<Enemys.Length; i++)
        {
            ITakeDamageable target = Enemys[i].GetComponent<ITakeDamageable>();
            if(target != null)
            {
                target.TakeDamage((int)bombPow);
            }
        }
        //Destroy(gameObject);
    }
}

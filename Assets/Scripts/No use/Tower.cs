using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public static Tower Instance; //Tower의 싱글톤 객체
    //데미지 표현할 UI
    public Transform damageUI; 
    public Image damageImage;
    public int initialHP = 10; //멤버변수, 필드
    int _hp = 0;
    public int HP //프로퍼티 
    {
        get
        {
            return _hp;
        }
        set{
            _hp = value;
            StopAllCoroutines(); //기존 진행 중인 코루틴 해제
            StartCoroutine(DamageEvent()); //깜박거림을 처리할 코루틴 함수 호출
            if (_hp <= 0)
            {
                Destroy(gameObject); // 타워의 체력이 0이되면 타워, 플레이어, 카메라가 모두 제거
            }
        }
    }
    public float damageTime = 0.1f;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this; //싱글톤 객체 값 할당
        }
    }

    void Start()
    {
        _hp = initialHP;
        float z = Camera.main.nearClipPlane + 0.1f;
        damageUI.parent = Camera.main.transform;
        damageUI.localPosition = new Vector3(0, 0, z);
        damageImage.enabled = false;
    }
    IEnumerator DamageEvent()
    {
        damageImage.enabled = true;
        yield return new WaitForSeconds(damageTime);
        damageImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

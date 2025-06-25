using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// 주의) 싱글톤 객체
public class BulletEffectSpawner : MonoBehaviour
{
    #region Variables
    public BulletEffect bulletEffectPrefab;

    public IObjectPool<BulletEffect> bulletEffectPool;  // Bullet Effect 오브젝트 풀
    #endregion

    #region Properties
    public static BulletEffectSpawner Instance { get; private set; }
    #endregion

    #region Unity Functions 
    private void Awake()
    {        
        if (Instance == null)
        {
            Instance = this;
            bulletEffectPool = new ObjectPool<BulletEffect>(
            createFunc: () =>
            {
                BulletEffect bulletEffect = Instantiate(bulletEffectPrefab, this.transform);    // BulletEffectPrefab을 바탕으로 생성
                return bulletEffect;
            },
            actionOnGet: (bulletEffect) => bulletEffect.gameObject.SetActive(true),             // bulletEffectPool.Get()시 실행 할 함수
            actionOnRelease: (bulletEffect) => bulletEffect.gameObject.SetActive(false),        // bulletEffectPool.Release()시 실행 할 함수
            actionOnDestroy: (bulletEffect) => Destroy(bulletEffect.gameObject),                // bulletEffectPool.Destroy()시 실행 할 함수
            collectionCheck: false,
            defaultCapacity: 10,                                                                // 초기 용량
            maxSize: 50                                                                         // 최대 생성 가능 사이즈
            );
        }
        else
        {
            Destroy(gameObject);   // 이미 인스턴스가 존재하면 현재 게임 오브젝트 파괴
            return;
        }
    }
    #endregion

    #region User Functions
    public void SpawnBulletEffect(Vector3 position, Vector3 direction)
    {
        // BulletEffect 생성 후, 위치 조정
        BulletEffect bulletEffect = bulletEffectPool.Get();
        bulletEffect.transform.position = position;
        bulletEffect.transform.forward = direction;

        // BulletEffect 내장된 파티클, 사운드 실행
        bulletEffect.Play();

        // 자동으로 릴리스 할 수 있게 코루틴 생성
        bulletEffect.StartCoroutine(bulletEffect.ReleaseAfterPlay());
    }

    public void Release(BulletEffect bulletEffect)
    {       
        bulletEffectPool.Release(bulletEffect);
    }
    #endregion
}
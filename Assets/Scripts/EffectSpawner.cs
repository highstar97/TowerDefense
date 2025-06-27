using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using static UnityEngine.ParticleSystem;
using static UnityEngine.UI.GridLayoutGroup;

public class EffectSpawner : MonoBehaviour
{
    #region Variables
    public Effect effectPrefab;

    public IObjectPool<Effect> effectPool;  // Effect 오브젝트 풀
    #endregion

    #region Unity Functions 
    private void Awake()
    {
        effectPool = new ObjectPool<Effect>(
            createFunc: () =>
            {
                Effect effect = Instantiate(effectPrefab, this.transform);    // EffectPrefab을 바탕으로 생성
                return effect;
            },
            actionOnGet: (bulletEffect) => bulletEffect.gameObject.SetActive(true),             // EffectPool.Get()시 실행 할 함수
            actionOnRelease: (bulletEffect) => bulletEffect.gameObject.SetActive(false),        // EffectPool.Release()시 실행 할 함수
            actionOnDestroy: (bulletEffect) => Destroy(bulletEffect.gameObject),                // EffectPool.Destroy()시 실행 할 함수
            collectionCheck: false,
            defaultCapacity: 10,                                                                // 초기 용량
            maxSize: 50                                                                         // 최대 생성 가능 사이즈
        );
    }
    #endregion

    #region User Functions
    public void SpawnEffect(Vector3 position, Vector3 direction)
    {
        // Effect 생성 후, 위치 조정
        Effect effect = effectPool.Get();
        effect.transform.position = position;
        effect.transform.forward = direction;

        // Effect 내장된 파티클, 사운드 실행
        effect.Play();

        // 자동으로 릴리스 할 수 있게 코루틴 생성
        StartCoroutine(ReleaseAfterPlay(effect));
    }

    public void Release(Effect effect)
    {       
        effectPool.Release(effect);
    }

    public IEnumerator ReleaseAfterPlay(Effect effect)
    {
        // 파티클과 오디오 둘 중 하나라도 플레이되고 있으면 대기
        while (effect.Particle.isPlaying || effect.AudioSource.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // 오브젝트 풀에 다시 반환
        effectPool.Release(effect);
    }
    #endregion
}
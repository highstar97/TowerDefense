using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    #region Variables
    public int damage = 3;                      // Explosion Damage;
    public float range = 3;                     // Explosion Range
    public LayerMask targetLayerMasks;          // Target Layer Masks;

    private bool isExploded;
    private EffectSpawner effectSpawner;        // Bomb Effect Spawner
    #endregion

    #region Unity Functions
    // 작성자 : 박규탁
    // 기  능 : 맵에 배치된 Explosion Effect Spawner 찾기
    private void Awake()
    {
        effectSpawner = GameObject.Find("Explosion Effect Spawner").GetComponent<EffectSpawner>();
    }

    // 작성자 : 박규탁
    // 기  능 : target Layer Masks에 등록된 타겟들에게 거리에 따른 데미지 주는 함수
    private void OnCollisionEnter(Collision collision)
    {
        Collider[] targets = Physics.OverlapSphere(this.transform.position, this.range, targetLayerMasks);
        foreach(Collider target in targets)
        {
            float distance = Vector3.Distance(this.transform.position, target.transform.position);

            ITakeDamageable damageable = target.GetComponent<ITakeDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(Mathf.Min((int)(damage / distance), damage));
            }
        }

        // Collision은 두 물체끼리 부딪혔을 때 총 2번 발동(수박 게임과 같은 이슈) → isExploded 변수 추가
        if(isExploded == false)
        {
            isExploded = true;
            effectSpawner.SpawnEffect(this.transform.position, this.transform.rotation.eulerAngles);
            Destroy(this.gameObject);
        }        
    }
    #endregion
}
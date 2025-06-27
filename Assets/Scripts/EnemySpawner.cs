using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float minSpawnTime = 2.0f;

    [SerializeField]
    private float maxSpawnTime = 5.0f;

    [SerializeField]
    List<Transform> spawnTransformList = new List<Transform>();

    [SerializeField]
    private Enemy enemyPrefab;     // Enemy Prefab

    private float expectedSpawnTime = 1.0f;

    private float elapsedTime;

    private IObjectPool<Enemy> enemyPool; //Enemy 오브젝트 풀

    private void Awake()
    {
        enemyPool = new ObjectPool<Enemy>(
          createFunc: () =>
          {
              Enemy enemy = Instantiate(enemyPrefab, this.transform);
              return enemy;
          },
          actionOnGet:(m_enemy) => { m_enemy.gameObject.SetActive(true); },
          actionOnRelease: (m_enemy) => m_enemy.gameObject.SetActive(false),
          actionOnDestroy: (m_enemy) => Destroy(m_enemy.gameObject),
          collectionCheck: false,
          defaultCapacity: 10,
          maxSize: 50
      );
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > expectedSpawnTime)
        {
            elapsedTime = 0.0f;
            SetNextSpawnTime();

            Vector3 spawnPosition = spawnTransformList[Random.Range(0, spawnTransformList.Count)].position;
            SpawnEnemy(spawnPosition, Vector3.forward);
        }
    }

    public void SpawnEnemy(Vector3 position , Vector3 direction)
    {
        Enemy enemy = enemyPool.Get();
        enemy.transform.position = position;
        enemy.transform.forward = direction;
    }

    public void Release(Enemy enemy)
    {
        enemyPool.Release(enemy);
    }

    private void SetNextSpawnTime()
    {
        expectedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
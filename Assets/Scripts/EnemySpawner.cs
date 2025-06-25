using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float minSpawnTime = 2.0f;

    [SerializeField]
    private float maxSpawnTime = 5.0f;

    [SerializeField]
    List<Transform> transformList = new List<Transform>();

    [SerializeField]
    private GameObject enemyPrefab;     // Enemy Prefab

    private float expectedSpawnTime = 1.0f;

    private float elapsedTime;
    #endregion

    #region Unity Functions    
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > expectedSpawnTime)
        {
            elapsedTime = 0.0f;
            SetNextSpawnTime();

            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = transformList[Random.Range(0, transformList.Count)].position;
        }
    }
    #endregion

    #region User Functions
    private void SetNextSpawnTime()
    {
        expectedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
    #endregion
}
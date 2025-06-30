using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 10;              // 코인 가치

    private bool isMovedToPlayer = false;
    private float moveToPlayerSpeed = 5f;   // 플레이어로 향하는 속도
    private float coinUpDownSpeed = 5f;     // 코인 위아래 움직이는 속도
    private float rotationSpeed = 90f;      // 코인 시계방향으로 회전하는 속도
    private Vector3 startPos;
    private Transform playerTransform;      // 플레이어 위치

    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;

        StartCoroutine(Co_MoveToPlayer());

        startPos = transform.position;

    }

    private void Update()
    {
        if(isMovedToPlayer == false)
        {
            // 위아래로 움직이면서, 시계방향으로 도는
            float y = Mathf.Sin(Time.time * coinUpDownSpeed) * 0.5f;
            transform.position = startPos + new Vector3(0, y, 0);

            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    /* 작성자: 임은성
     * 기능: 생성된 후 3초 뒤부터 코인이 플레이어를 향해 이동하며,
     *       일정 거리(0.6f) 이내로 접근 시 플레이어가 코인을 획득하고 코인을 파괴함
     */
    private IEnumerator Co_MoveToPlayer()
    {
        yield return new WaitForSeconds(3.0f);
        isMovedToPlayer = true;

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveToPlayerSpeed * Time.deltaTime);
            yield return null;

            if (Vector3.Distance(transform.position, playerTransform.position) < 0.6f)
            {
                CoinManager.Instance.AddCoin(coinValue);
                Destroy(gameObject);
                yield break;
            }
        }
    }
}
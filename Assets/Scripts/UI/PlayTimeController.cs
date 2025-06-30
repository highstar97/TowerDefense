using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayTimeController : MonoBehaviour
{
    [SerializeField]
    public float initTime = 300f;
    public float remainedTime = 0f;

    [SerializeField]
    UnityEvent<float> m_changedPlayTimeEvent;

    private Coroutine timerCoroutine;

    void Start()
    {
        remainedTime = initTime;
        StartTime();
    }

    public void StartTime()
    {
        timerCoroutine = StartCoroutine(CoTimer()); // countTime 타이머
    }

    public void StopTime()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }


    /* 작성자: 임은성
     * 기능: 남은 시간이 0이 될 때까지 1초마다 감소시키고,
     *       시간이 모두 지나면 플레이어 승리 처리 후 게임 종료
    */

    IEnumerator CoTimer()
    {
        while (remainedTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainedTime -= 1f;
            m_changedPlayTimeEvent.Invoke(remainedTime);
        }
        remainedTime = 0f;
        m_changedPlayTimeEvent.Invoke(remainedTime);

        GameManager.Instance.isPlayerWin = true;
        GameManager.Instance.GameOver();
        StopTime();
    }
}
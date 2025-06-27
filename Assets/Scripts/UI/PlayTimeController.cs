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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayTimeController : MonoBehaviour
{
    [SerializeField]
    public float countTime = 300f;

    [SerializeField]
    UnityEvent<float> m_changedPlayTimeEvent;

    private Coroutine timerCoroutine;

    void Start()
    {
        StartTime();
    }

    public void StartTime()
    {
        timerCoroutine = StartCoroutine(CoTimer(300f)); //5분 타이머
    }

    public void StopTime()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    IEnumerator CoTimer(float time)
    {
        countTime = time;

        while (countTime > 0)
        {
            yield return new WaitForSeconds(1f);
            countTime -= 1f;
            m_changedPlayTimeEvent.Invoke(countTime);
        }
        countTime = 0f;
        m_changedPlayTimeEvent.Invoke(countTime);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayTimeController : MonoBehaviour
{
    [SerializeField]
    public float CountTime = 300f;

    [SerializeField]
    UnityEvent<float> m_changedPlayTimeEvent;

    private Coroutine timerCoroutine;

    void Start()
    {
        StartTime();
    }

    public void StartTime()
    {
        Debug.Log("StartTime Called");
        timerCoroutine = StartCoroutine(CoTimer(300f)); //5분 카운트 다운
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
        float currentTime = time;

        while (currentTime > 0)
        {

            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
            m_changedPlayTimeEvent.Invoke(currentTime);
        }
        m_changedPlayTimeEvent.Invoke(0f);

    }
  

}

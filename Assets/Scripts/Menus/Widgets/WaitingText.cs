using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextComponent;

    private float m_WaitingDuration;

    private WaitForSecondsRealtime m_WaitDuration = new(1f);

    public void StartTimer(string message, float wait)
    {
        SetTextWithTimeInternal(message, wait);
        SetActiveState(true);
        
        StartCoroutine(TimerRoutine(message, wait));
    }

    private IEnumerator TimerRoutine(string textMessage,float timer)
    {
        float timeForWait = timer;

        while (timeForWait > 0)
        {
            SetTextWithTimeInternal(textMessage, timeForWait);
            yield return m_WaitDuration;
            timeForWait--;
        }
        SetActiveState(false);
    }

    private void SetTextWithTimeInternal(string message, float seconds)
    {
        string text = seconds > 1 ?"seconds" : "second";
        m_TextComponent.text = $"{message} in {seconds} {text}";
    }

    private void SetActiveState(bool status)
    {
        m_TextComponent.gameObject.SetActive(status);
    }
}

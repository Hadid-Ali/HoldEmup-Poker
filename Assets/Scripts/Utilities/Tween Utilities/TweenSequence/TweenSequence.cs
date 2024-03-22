using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TweenObject
{
    [field: SerializeField] public BaseTween Tween { get; private set; }
    [field: SerializeField] public float WaitBeforePlayingTween { get; private set; }
}

public class TweenSequence : MonoBehaviour
{
    [SerializeField] protected bool m_IsLoop = false;
    [SerializeField] protected List<TweenObject> m_TweenObjects;

    private void Start()
    {
        StartCoroutine(TweenRoutine());
    }
    
    private IEnumerator TweenRoutine()
    {
        for (int i = 0; i < m_TweenObjects.Count; i++)
        {
            yield return new WaitForSeconds(m_TweenObjects[i].WaitBeforePlayingTween);
            m_TweenObjects[i].Tween.PerformTweenOperation();
            yield return new WaitForSeconds(m_TweenObjects[i].Tween.SequenceDuration());

            if (m_IsLoop)
            {
           //     if (i + 1 == m_TweenObjects.Count)
             //       i = 0;
            }
            Debug.LogError(i);
        }
    }
}

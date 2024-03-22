using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TweenUtilityData
{
    [SerializeField] private Vector3 m_TargetPosition;
    [SerializeField] private float m_TravelTime;

    public Vector3 Target => m_TargetPosition;
    public float TargetDuration => m_TravelTime;
}

public abstract class BaseTween : MonoBehaviour
{
    [SerializeField] protected TweenUtilityData m_TweenData;
    [SerializeField] private bool m_PlayOnEnable;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        if (m_PlayOnEnable)
        {
            PerformTweenOperation();
        }
    }

    public float SequenceDuration() => m_TweenData.TargetDuration;

    protected abstract void Init();
    public abstract void PerformTweenOperation();
}

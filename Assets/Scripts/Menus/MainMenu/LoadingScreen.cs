using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts._UXUI;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class LoadingScreen : UIMenuBase
{
    [Header("Component Refs")]
    [SerializeField] private AnimatedTextWidget m_AnimatedTextWidget;
    [SerializeField] private WaitingText m_WaitingText;
    
    [Header("UI Refs")]
    [SerializeField] private TextMeshProUGUI m_LobbyStatusText;

    private void OnEnable()
    {
        GameEvents.MenuEvents.NetworkStatusUpdated.Register(UpdateLobbyStatus);
        GameEvents.MenuEvents.TimeBasedActionRequested.Register(OnTimeBasedActionRequested);
    }

    private void OnDisable()
    {
        GameEvents.MenuEvents.NetworkStatusUpdated.UnRegister(UpdateLobbyStatus);
        GameEvents.MenuEvents.TimeBasedActionRequested.UnRegister(OnTimeBasedActionRequested);
    }

    private void UpdateLobbyStatus(string status)
    {
        m_LobbyStatusText.text = status;
        m_AnimatedTextWidget.AnimateText(status);
    }

    private void OnTimeBasedActionRequested(string message, float wait)
    {
        m_WaitingText.StartTimer(message, wait);
    }
}

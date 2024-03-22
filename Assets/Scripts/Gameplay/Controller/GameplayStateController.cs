using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayStateController : MonoBehaviour
{
    [SerializeField] private GameplayState m_GameplayState = GameplayState.Casino_View;
    [SerializeField] private float m_WaitBeforeCardsFocus = 2f;

    private bool m_IsGameCompleted = false;
    
    void Start()
    {
       GameEvents.GameFlowEvents.RoundStart.Raise();
    }

    private void OnEnable()
    {
        GameEvents.GameFlowEvents.RoundStart.Register(LoadCardsView);
        GameEvents.GameFlowEvents.MatchOver.Register(OnMatchOver);
    }

    private void OnDisable()
    {
        GameEvents.GameFlowEvents.RoundStart.UnRegister(LoadCardsView);
        GameEvents.GameFlowEvents.MatchOver.UnRegister(OnMatchOver);
    }

    private void OnMatchOver()
    {
        m_IsGameCompleted = true;
    }

    private void LoadCardsView()
    {
        ChangeState(GameplayState.Casino_View);
        
        if (m_IsGameCompleted)
            return;
        
        Invoke(nameof(ChangeToCardsView), m_WaitBeforeCardsFocus);
    }
    
    private void ChangeToCardsView()
    {
        ChangeState(GameplayState.Cards_View);
    }

    private void ChangeState(GameplayState gameplayState)
    {
        if (m_GameplayState == gameplayState)
            return;
        
        m_GameplayState = gameplayState;
        GameEvents.GameplayEvents.GameplayStateSwitched.Raise(gameplayState);
        OnGameStateChanged(gameplayState);
    }

    private void OnGameStateChanged(GameplayState gameplayState)
    {
        Debug.LogError($"Gameplay State {gameplayState}");
    }
}

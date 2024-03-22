using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundCompleteMenu : UIMenuBase
{
    [Header("Components")] 
    [SerializeField] private TMP_Text m_RewardText;
    [SerializeField] private Button m_RestartButton;

    private void Start()
    {
        m_RestartButton.onClick.AddListener(OnRestartTap);
    }

    private void OnEnable()
    {
        GameEvents.GameplayUIEvents.PlayerRewardReceived.Register(OnPlayerRewardReceived);
        GameEvents.GameFlowEvents.RoundStart.Register(DisableMenu);
    }

    private void OnDisable()
    {
        GameEvents.GameplayUIEvents.PlayerRewardReceived.UnRegister(OnPlayerRewardReceived);
        GameEvents.GameFlowEvents.RoundStart.UnRegister(DisableMenu);
    }

    private void DisableMenu()
    {
        SetMenuActiveState(false);
    }

    private void OnPlayerRewardReceived(int reward)
    {
        m_RewardText.text = $"You Received {reward} Points in this Round";
    }

    private void OnRestartTap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameEvents.GameFlowEvents.RestartRound.Raise();
        }
        else
        {
            ChangeMenuState(MenuName.LoadingScreen);
        }
    }
}

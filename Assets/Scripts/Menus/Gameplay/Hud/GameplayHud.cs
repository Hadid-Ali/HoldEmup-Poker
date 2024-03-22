using System;
using System.Collections;
using Photon.Pun;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;

public class GameplayHud : MenusController
{
    [Header("Refs")]
    
    [SerializeField] private GameObject m_ButtonsContainer;
    [SerializeField] private GameObject m_WaitingText;
    
    [Header("UI Components")]
    
    [SerializeField] private Button m_SubmitButton;
    [SerializeField] private Button m_DisconnectButton;
    
    [SerializeField] private TMP_Text m_TotalScore;
    [SerializeField] private GameObject m_DisconnectedLabel;

    [SerializeField] private GameObject m_InputBlocker;

    private bool m_ShowMatchoverMenu = false;
    
    private void Start()
    {
        m_SubmitButton.onClick.AddListener(SubmitCards);
        m_DisconnectButton.onClick.AddListener(Disconnect);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameEvents.NetworkEvents.NetworkDisconnectedEvent.Register(OnNetworkDisconnected);
        GameEvents.GameFlowEvents.RoundStart.Register(AllowGameplayInputs);
        GameEvents.GameFlowEvents.MatchOver.Register(OnMatchOver);
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        GameEvents.NetworkEvents.NetworkDisconnectedEvent.UnRegister(OnNetworkDisconnected);
        GameEvents.GameFlowEvents.RoundStart.UnRegister(AllowGameplayInputs);
        GameEvents.GameFlowEvents.MatchOver.UnRegister(OnMatchOver);
    }

    public void ShowScoreOnUI()
    {
        m_TotalScore.text = $"{GameData.RuntimeData.TOTAL_PLAYER_SCORE} pts";
    }

    public void ShowResultMenu()
    {
        Debug.LogError("Show Result Menu");
        SetMenuState(m_ShowMatchoverMenu ? MenuName.MatchCompleteMenu : MenuName.RoundCompleteMenu);
    }
    
    private void OnMatchOver()
    {
        m_ShowMatchoverMenu = true;
    }
    
    private void OnNetworkDisconnected()
    {
    //    m_DisconnectedLabel.SetActive(true);
    }

    private void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    private void SubmitCards()
    {
        // try
        // {
            GameEvents.GameplayUIEvents.SubmitDecks.Raise();
            SetGameplayInputStatus(false);
        // }
        // catch (Exception e)00
        // {
        //     Debug.LogError(e.StackTrace);
        // }
    }
    
    private void AllowGameplayInputs()
    {
        SetGameplayInputStatus(true);
    }

    private void SetGameplayInputStatus(bool status)
    {
        m_ButtonsContainer.SetActive(status);
        m_InputBlocker.SetActive(!status);
        m_WaitingText.SetActive(!status);
    }
}

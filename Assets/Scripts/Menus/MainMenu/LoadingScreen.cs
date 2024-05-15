using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts._UXUI;
using Photon.Realtime;
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
        GameEvents.NetworkEvents.NetworkStatus.Register(UpdateLobbyStatus);
        GameEvents.MenuEvents.TimeBasedActionRequested.Register(OnTimeBasedActionRequested);
        
        GameEvents.NetworkEvents.RoomJoinFailed.Register(OnRoomJoinFailed);
        
        GameEvents.NetworkEvents.LobbyJoined.Register(OnCreateRoom);

    }
    private void OnDisable()
    {
        GameEvents.NetworkEvents.NetworkStatus.UnRegister(UpdateLobbyStatus);
        GameEvents.MenuEvents.TimeBasedActionRequested.UnRegister(OnTimeBasedActionRequested);
        
        GameEvents.NetworkEvents.RoomJoinFailed.UnRegister(OnRoomJoinFailed);

        GameEvents.NetworkEvents.LobbyJoined.UnRegister(OnCreateRoom);
    }

    private void OnRoomJoinFailed()
    {
        int maxPlayerCount = GameData.MetaData.MaxPlayersLimit;
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = (byte)maxPlayerCount,
            IsOpen = true,
            IsVisible = true,
            EmptyRoomTtl = 100,
        };

        GameEvents.NetworkEvents.PlayerRoomCreation.Raise(roomOptions);
        GameData.SessionData.CurrentRoomPlayersCount = maxPlayerCount;
    }



    private void OnCreateRoom()
    {
        GameEvents.NetworkEvents.PlayerCharacterSelected.Raise();
        
       
    }

    private void OnServerDisconnect(RegionConfig obj)
    {
        ChangeMenuState(MenuName.LoginScreen);
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

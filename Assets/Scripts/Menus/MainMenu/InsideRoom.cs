using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InsideRoom : UIMenuBase
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button button;
    
    private void OnEnable()
    {
        button.onClick.AddListener(OnStartMatchClicked);
        GameEvents.MenuEvents.PlayersListUpdated.Register(UpdatePlayerList);
        GameEvents.NetworkEvents.PlayerJoinedRoom.Register(OnPlayerJoinRoom);
        GameEvents.NetworkEvents.NetworkStatus.Register(UpdateLobbyStatus);
    }
    private void OnDisable()
    {
        GameEvents.MenuEvents.PlayersListUpdated.UnRegister(UpdatePlayerList);
        GameEvents.NetworkEvents.PlayerJoinedRoom.UnRegister(OnPlayerJoinRoom);
        GameEvents.NetworkEvents.NetworkStatus.UnRegister(UpdateLobbyStatus);
    }

    private void UpdateLobbyStatus(string obj)
    {
        statusText.SetText(obj);
    }

    private void OnPlayerJoinRoom(bool val)
    {
        if(button)
            button.gameObject.SetActive(val);
    }

    private void OnStartMatchClicked()
    {
        GameEvents.MenuEvents.MatchStartRequested.Raise();
        button.interactable = false;
    }


    private void OnValidate()
    {
        if(!PhotonNetwork.IsMasterClient)
            return;
        
        button.interactable = PhotonNetwork.PlayerList.Length > 1;
    }
    
    private void UpdatePlayerList(List<string> Players)
    {
        var players = Players.Aggregate(String.Empty, (current, v) => current + $"\n {v}");

        textMeshPro.text = $"Players Joined : {players}";

        button.interactable = Players.Count > 1;
        
        statusText.gameObject.SetActive(!PhotonNetwork.IsMasterClient);
    }
}



using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsideRoom : UIMenuBase
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button _button;
    
    private void OnEnable()
    {
        _button.onClick.AddListener(()=> { GameEvents.MenuEvents.MatchStartRequested.Raise();});
        GameEvents.MenuEvents.PlayersListUpdated.Register(UpdatePlayerList);
        
        GameEvents.NetworkEvents.PlayerJoinedRoom.Register((bool b)=> _button.gameObject.SetActive(b));
        GameEvents.NetworkEvents.NetworkStatus.Register(UpdateLobbyStatus);
    }

    private void UpdateLobbyStatus(string obj)
    {
        statusText.SetText(obj);
    }

    private void OnDisable()
    {
        GameEvents.MenuEvents.PlayersListUpdated.UnRegister(UpdatePlayerList);
        GameEvents.NetworkEvents.PlayerJoinedRoom.UnRegister((bool b)=> _button.gameObject.SetActive(b));
        GameEvents.NetworkEvents.NetworkStatus.UnRegister(UpdateLobbyStatus);
    }

    private void OnValidate()
    {
        if(!PhotonNetwork.IsMasterClient)
            return;
        
        _button.interactable = PhotonNetwork.PlayerList.Length > 1;
    }
    
    private void UpdatePlayerList(List<string> Players)
    {
        print($"Function working");
        string players = String.Empty;

        foreach (var v in Players)
        {
            players += $"\n {v}";
            print($"{v}");
        }
        _textMeshPro.text = $"Players Joined : {players}";

        _button.interactable = Players.Count > 1;
        
        statusText.gameObject.SetActive(!PhotonNetwork.IsMasterClient);
    }
}



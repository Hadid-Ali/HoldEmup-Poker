using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsideRoom : UIMenuBase
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Button _button;

    private void Awake()
    {
        GameEvents.NetworkEvents.OnRoomJoined.Register(AddPlayer);
        GameEvents.NetworkEvents.OnPlayerJoined.Register(AddPlayer);
            
        _button.onClick.AddListener(()=> { GameEvents.NetworkEvents.OnStartMatch.Raise();});
    }

    private void OnValidate()
    {
        if(!PhotonNetwork.IsMasterClient)
            return;
        
        _button.interactable = PhotonNetwork.PlayerList.Length > 1;
    }

    private void OnDestroy()
    {
        GameEvents.NetworkEvents.OnRoomJoined.UnRegister(AddPlayer);
        GameEvents.NetworkEvents.OnPlayerJoined.UnRegister(AddPlayer);
    }
    
    private void AddPlayer()
    {
        _button.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        Player[] playersA = PhotonNetwork.PlayerList;
        _textMeshPro.text = $"Players Joined : {playersA.Length}";

        _button.interactable = playersA.Length > 1;
    }
}

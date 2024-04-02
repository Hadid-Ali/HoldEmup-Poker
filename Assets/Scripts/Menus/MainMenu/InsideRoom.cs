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
        GameEvents.NetworkEvents.OnPlayerRoomActivity.Register(AddPlayer);
            
        _button.onClick.AddListener(()=> { GameEvents.NetworkEvents.OnStartMatch.Raise();});
    }
    private void OnDestroy()
    {
        GameEvents.NetworkEvents.OnPlayerRoomActivity.UnRegister(AddPlayer);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        AddPlayer();
    }

    private void AddPlayer()
    {
        Player[] playersA = PhotonNetwork.PlayerList;
            
        string playerString = String.Empty;

        for (int i = 0; i < playersA.Length; i++)
        {
            playerString = $"Player Joined : {playersA[i].NickName} \n";
        }
        print(playerString);

        _textMeshPro.text = playerString;
    }
}

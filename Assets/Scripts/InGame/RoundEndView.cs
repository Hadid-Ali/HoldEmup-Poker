using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundEndView : MonoBehaviour
{
    [SerializeField] private PhotonView view;
    [SerializeField] private DialogBox box;

    private void Awake()
    {
        GameEvents.NetworkGameplayEvents.OnPlayerWin.Register(OnPlayerWin);
        GameEvents.NetworkGameplayEvents.OnRoundEnd.Register(OnResetView);
    }

    private void OnResetView()
    {
        
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.OnPlayerWin.UnRegister(OnPlayerWin);
        GameEvents.NetworkGameplayEvents.OnRoundEnd.UnRegister(OnResetView);
    }

    private void OnPlayerWin(int arg1, int arg2)
    {
        string text = PhotonNetwork.LocalPlayer.ActorNumber == arg1
            ? $"Congratulation, You've won  {arg2} coins"
            : "Sorry, you did not win anything this time";

        box.Initialize(text, OnButtonClick);
    }

    private void OnButtonClick()
    {
        view.RPC(nameof(BroadCastConsent), RpcTarget.MasterClient);
    }

    [PunRPC]
    private void BroadCastConsent()
    {
        GameEvents.NetworkGameplayEvents.OnContinueConsentCollected.Raise();
    }
}

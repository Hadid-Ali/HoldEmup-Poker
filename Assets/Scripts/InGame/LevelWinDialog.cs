using System;
using DG.Tweening;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelWinDialog : MonoBehaviour
{
    [SerializeField] private PhotonView view;
    [SerializeField] private GameObject levelWinDialog;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;
    [SerializeField] private CanvasGroup _group;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClick);
        GameEvents.NetworkGameplayEvents.OnPlayerWin.Register(OnPlayerWin);
        GameEvents.NetworkGameplayEvents.OnRoundEnd.Register(OnResetView);
    }

    private void OnResetView()
    {
        levelWinDialog.SetActive(false);
        _group.alpha = 0;
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.OnPlayerWin.UnRegister(OnPlayerWin);
        GameEvents.NetworkGameplayEvents.OnRoundEnd.UnRegister(OnResetView);
    }

    private void OnPlayerWin(int arg1, int arg2)
    {
        text.SetText(PhotonNetwork.LocalPlayer.ActorNumber == arg1
            ? $"Congratulation, You've won  {arg2} coins"
            : "Sorry, you did not win anything this time");

        levelWinDialog.SetActive(true);
        _group.DOFade(1, 1);

        print($"Local Player has won {PhotonNetwork.LocalPlayer.ActorNumber == arg1} : {PhotonNetwork.LocalPlayer.NickName}");
    }

    private void OnButtonClick()
    {
        button.interactable = false;
        view.RPC(nameof(BroadCastConsent), RpcTarget.MasterClient);
    }

    [PunRPC]
    private void BroadCastConsent()
    {
        GameEvents.NetworkGameplayEvents.OnContinueConsentCollected.Raise();
    }
}

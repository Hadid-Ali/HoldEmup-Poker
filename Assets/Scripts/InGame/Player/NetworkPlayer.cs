using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public enum BetAction
{
    UnSelected,
    Fold,
    Call,
    Raise,
    Check,
    AllIn,
    SmallBlind,
    BigBlind
}
public class NetworkPlayer : MonoBehaviourPun
{
    [field: SerializeField] public PhotonView PhotonView {private set; get; }
    [field: SerializeField] public PlayerCards PlayerCards{ private set; get; }
    [field: SerializeField] public PlayerCredit PlayerCredit{private set; get; }

    private bool _hasFolded;
    public bool hasFolded
    {
        set
        {
            _hasFolded = value;
            PhotonView.RPC(nameof(HasFolded_RPC), RpcTarget.All, _hasFolded);
        }
        get => _hasFolded;
    }

    public string nickName;
    public int id;
    public BetAction lastBetAction;
    public int betAmount = 2;
    public bool IsLocalPlayer => PhotonView.IsMine;
    
    private bool _canMakeTurn;

    public static Action<NetworkPlayer> OnPlayerSpawn;
    public static Action<bool> OnEnableTurn;
    public static Action<BetAction, bool> OnEnableAction;
    private void Awake()
    {
        GamePlayButtons.OnPlayerActionSubmit += OnActionSubmit;
    }

    private void OnDestroy()
    {
        GamePlayButtons.OnPlayerActionSubmit -= OnActionSubmit;
    }

    private void Start() => Invoke(nameof(OnNetworkSpawn), 1.5f);
    public void OnNetworkSpawn() => OnPlayerSpawn?.Invoke(this);
    public void SyncInformationGlobally() => 
        PhotonView.RPC(nameof(SyncInformation), RpcTarget.All, id,nickName);

    public void DealCards()
    {
        CardData card1 = DecksHandler.GetRandomCard();
        CardData card2 = DecksHandler.GetRandomCard();
        print($"Cards Dealt : {card1} : {card2}");
        
        PlayerCards.SetPocketCards(card1,card2);
    }

    public void OnLocalPlayerRaiseSlideUpdate(int min, int max) =>
        PhotonView.RPC(nameof(UpdateRaiseSlider), RpcTarget.All, min, max);
    
    private void OnActionSubmit(BetAction obj, int _betAmount)
    {
        if(!IsLocalPlayer)
            return;
            
        lastBetAction = obj;
        betAmount = _betAmount;
        OnEnableTurn.Invoke(false);
        
        PhotonView.RPC(nameof(SetSelectedBetActionServerRpc), RpcTarget.All, (int) obj, id, betAmount);
    }
    public void EnableTurn(bool val)
    {
        PhotonView.RPC(nameof(EnableTurn_RPC), RpcTarget.All, val, id);
    }

    public void EnableAction(BetAction action, bool val)
    {
        PhotonView.RPC(nameof(EnableActionRpc), RpcTarget.All, (int)action, val);
    }

    public void SetAction(BetAction action)
    {
        lastBetAction = action;
        PhotonView.RPC(nameof(OnPlayerAction), RpcTarget.All, lastBetAction.ToString());
    }

    public override string ToString()
    {
        return $"Nick: '{nickName}', ID: '{id}'";
    }
    
    #region RPC

    [PunRPC]
    private void UpdateRaiseSlider(int min, int max)
    {
        if(IsLocalPlayer)
            GameEvents.NetworkPlayerEvents.OnSetPlayerRaiseLimits.Raise(min, max);
    }

    [PunRPC]
    private void EnableTurn_RPC(bool b, int _id)
    {
        if (id == _id)
        {
            if(IsLocalPlayer)
                OnEnableTurn?.Invoke(b);
        } 
        GameEvents.NetworkPlayerEvents.OnPlayerTurn.Raise(_id,b);
    }

    [PunRPC]
    private void HasFolded_RPC(bool b)
    {
        _hasFolded = b;
    }
    [PunRPC]
    private void SyncInformation(int id, string nickName)
    {
        this.nickName = nickName;
        this.id = id;
    }
    
    [PunRPC]
    private void SetSelectedBetActionServerRpc(int playerAction, int _id, int _betAmount)
    {
        lastBetAction = (BetAction) playerAction;
        hasFolded = lastBetAction == BetAction.Fold;
        
        betAmount = _betAmount;
        
        if(!PhotonNetwork.IsMasterClient)
            return;

        if (id == _id && (BetAction) playerAction != BetAction.UnSelected)
            Betting.PlayerEndBettingEvent?.Invoke(new BetActionInfo(this, lastBetAction, betAmount));
        
        GameEvents.NetworkPlayerEvents.OnPlayerActionPop.Raise(id,lastBetAction.ToString());
        PhotonView.RPC(nameof(OnPlayerAction), RpcTarget.All,lastBetAction.ToString());
    }

    [PunRPC]
    private void OnPlayerAction(string text)
    {
        GameEvents.NetworkPlayerEvents.OnPlayerActionPop.Raise(id,text);
    }
    
    [PunRPC]
    private void EnableActionRpc(int i, bool val)
    {
        if(IsLocalPlayer)
            OnEnableAction?.Invoke((BetAction)i, val);
    }
    
    #endregion


}
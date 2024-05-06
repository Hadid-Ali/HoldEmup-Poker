using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public enum BetAction
{
    UnSelected,
    Fold,
    Call,
    Raise,
    Check,
    AllIn
}
public class NetworkPlayer : MonoBehaviourPun
{
    [SerializeField] private PhotonView _photonView;
    public bool isOnTurn;
    private bool _hasFolded;
    public bool HasFolded
    {
        get => _hasFolded;
        set => _photonView.RPC(nameof(SyncInformation), RpcTarget.All, value);
    }

    public string nickName;
    public int id;
    public BetAction lastBetAction;
    public int betAmount = 2;
    public bool IsLocalPlayer => _photonView.IsMine;
    public bool IsBroke => totalCredit <= 0;
    
    public CardData pocketCard1;
    public CardData pocketCard2;
    
    public int totalCredit = 1000;
    private bool _canMakeTurn;

    public static Action<NetworkPlayer> OnPlayerSpawn;
    public static Action<bool> OnEnableTurn;
    public static Action<BetAction, bool> OnEnableAction;
    private void Start()
    {
        Invoke(nameof(OnNetworkSpawn), 1.5f);
        GamePlayButtons.OnPlayerActionSubmit += OnActionSubmit;
        GameEvents.NetworkGameplayEvents.ExposePocketCardsLocally.Register(ExposeCardsLocally);
        GameEvents.NetworkGameplayEvents.OnShowDown.Register(SubmitCards);
    }

    private void SubmitCards()
    {
        List<CardData> cards = new ()
        {
            pocketCard1,
            pocketCard2
        };

        GameEvents.NetworkGameplayEvents.NetworkSubmitRequest.Raise(new NetworkDataObject(cards, id));
    }

    private void OnDestroy()
    {
        GamePlayButtons.OnPlayerActionSubmit -= OnActionSubmit;
        GameEvents.NetworkGameplayEvents.ExposePocketCardsLocally.UnRegister(ExposeCardsLocally);
        GameEvents.NetworkGameplayEvents.OnShowDown.UnRegister(SubmitCards);
    }

    public void OnNetworkSpawn() => OnPlayerSpawn?.Invoke(this);
    public void ExposeCardsLocally() => _photonView.RPC(nameof(SyncInformation), RpcTarget.All);
    public void SyncInformationGlobally() => _photonView.RPC(nameof(SyncInformation), RpcTarget.All, id,nickName);
    public void DealCards(NetworkPlayer player) => player.SetPocketCards(DecksHandler.GetRandomCard(),DecksHandler.GetRandomCard(), player.id);

    public void OnLocalPlayerRaiseSlideUpdate(int min, int max) =>
        _photonView.RPC(nameof(UpdateRaiseSlider), RpcTarget.All, min, max);
    public void SetBetAction(BetAction betAction)
    {
        _photonView.RPC(nameof(SetSelectedBetActionServerRpc), RpcTarget.All, (int) betAction, id, betAmount);
    } 
    public void SubCredit(int val)
    {
        if(val > totalCredit)
            return;
        
        totalCredit -= val;  
        _photonView.RPC(nameof(SyncInformation), RpcTarget.All, totalCredit);
    }

    public void AddCredit(int val)
    {
        totalCredit += val;  
        _photonView.RPC(nameof(SyncInformation), RpcTarget.All, totalCredit);
    } 
    private void OnActionSubmit(BetAction obj, int _betAmount)
    {
        if(!IsLocalPlayer)
            return;
            
        lastBetAction = obj;
        betAmount = _betAmount;
        OnEnableTurn.Invoke(false);
        SetBetAction(obj);
    }
    public void EnableTurn(bool val)
    {
        _photonView.RPC(nameof(EnableTurn_RPC), RpcTarget.All, val, id);
    }

    public void EnableAction(BetAction action, bool val)
    {
        _photonView.RPC(nameof(EnableActionRpc), RpcTarget.All, (int)action, val);
    }

    public void SetPocketCards(CardData card1, CardData card2, int playerId)
    {
        int[] binaryData1 = card1.ConvertToIntArray();
        int[] binaryData2 = card2.ConvertToIntArray();
        
        _photonView.RPC(nameof(SyncPocketCards), RpcTarget.All, binaryData1,binaryData2,playerId);
    } 
    
    private void OnBetInputFieldValueChanged(int value)
    {
        SetBetInputFieldValueServerRpc(value);
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
            GameEvents.GameplayEvents.OnLocalPlayerRaise.Raise(min, max);
    }

    [PunRPC]
    private void EnableTurn_RPC(bool b, int _id)
    {
        if (id == _id)
        {
            isOnTurn = b;
            if(IsLocalPlayer)
                OnEnableTurn?.Invoke(b);
        }
        
        GameEvents.NetworkGameplayEvents.OnUpdatePlayersView.Raise();
    }

    [PunRPC]
    private void SyncInformation(bool b)
    {
        _hasFolded = b;
    }
    [PunRPC]
    private void SyncInformation(int id, string nickName)
    {
        this.nickName = nickName;
        this.id = id;
        
        GameEvents.NetworkGameplayEvents.OnUpdatePlayersView.Raise();
    }
    [PunRPC]
    private void SyncInformation(int credits)
    {
        this.totalCredit = credits;
        
        GameEvents.NetworkGameplayEvents.OnUpdatePlayersView.Raise();
    }
    [PunRPC]
    private void SyncInformation()
    {
        if(!IsLocalPlayer)
            return;
        
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.Raise(pocketCard1,pocketCard2);
    }
    
    [PunRPC]
    private void SetBetInputFieldValueServerRpc(int value) => betAmount = value;
    
    [PunRPC]
    private void SetSelectedBetActionServerRpc(int playerAction, int _id, int _betAmount)
    {
        print($"Checking : {id}");
        lastBetAction = (BetAction) playerAction;
        _hasFolded = lastBetAction == BetAction.Fold;
        betAmount = _betAmount;
        
        if(!PhotonNetwork.IsMasterClient)
            return;

        if (id == _id && (BetAction) playerAction != BetAction.UnSelected)
            Betting.PlayerEndBettingEvent?.Invoke(new BetActionInfo(this, lastBetAction, betAmount));
        
        GameEvents.NetworkGameplayEvents.OnUpdatePlayersView.Raise(); 
    }
    
    [PunRPC]
    private void SyncPocketCards(int[] binaryCardData1, int[] binaryCardData2, int playerID)
    {
        if(playerID != id)
            return;
        
        pocketCard1 = CardData.ConvertIntArrayToCardData(binaryCardData1);
        pocketCard2 = CardData.ConvertIntArrayToCardData(binaryCardData2);
    }
    
    [PunRPC]
    private void EnableActionRpc(int i, bool val)
    {
        if(IsLocalPlayer)
            OnEnableAction?.Invoke((BetAction)i, val);
    }
    
    #endregion


}
using System;
using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

public enum BetAction
{
    UnSelected,
    Fold,
    Call,
    Raise,
    Check
}
public class NetworkPlayer : MonoBehaviourPun
{
    [SerializeField] private PhotonView _photonView;
    
    public string nickName;
    public int id;
    [FormerlySerializedAs("lastPlayerAction")] public BetAction lastBetAction;
    public int betAmount = 2;
    public bool IsLocalPlayer => _photonView.IsMine;
    
    public CardData pocketCard1;
    public CardData pocketCard2;
    
    private int _totalCredit;
    private bool _canMakeTurn;

    public static Action<NetworkPlayer> OnPlayerSpawn;
    public static Action<bool> OnEnableTurn;
    private void Start()
    {
        Invoke(nameof(OnNetworkSpawn), 1.5f);
        GameEvents.NetworkGameplayEvents.ExposePocketCardsLocally.Register(ExposeCardsLocally);
        TurnSubmitButton.OnPlayerActionSubmit += OnActionSubmit;
        
    }
    private void OnDestroy()
    {
        TurnSubmitButton.OnPlayerActionSubmit -= OnActionSubmit;
        GameEvents.NetworkGameplayEvents.ExposePocketCardsLocally.UnRegister(ExposeCardsLocally);
    }

    public void OnNetworkSpawn() => OnPlayerSpawn?.Invoke(this);
    public void ExposeCardsLocally() => _photonView.RPC(nameof(SyncInformation), RpcTarget.All);
    public void SyncInformationGlobally() => _photonView.RPC(nameof(SyncInformation), RpcTarget.All, id,nickName);
    public void DealCards(NetworkPlayer player) => player.SetPocketCards(DecksHandler.GetRandomCard(),DecksHandler.GetRandomCard(), player.id);


    public void SetBetAction(BetAction betAction)
    {
        _photonView.RPC(nameof(SetSelectedBetActionServerRpc), RpcTarget.All, (int) betAction, id);
    } 
    public void SubCredit(int val)
    {
        _totalCredit -= val;  
        _photonView.RPC(nameof(SyncInformation), RpcTarget.All, _totalCredit);
    }

    public void AddCredit(int val)
    {
        _totalCredit += val;  
        _photonView.RPC(nameof(SyncInformation), RpcTarget.All, _totalCredit);
    } 
    private void OnActionSubmit(BetAction obj)
    {
        lastBetAction = obj;
        
        OnEnableTurn.Invoke(false);
        SetBetAction(obj);
    }
    public void EnableTurn(bool val)
    {
        _photonView.RPC(nameof(EnableTurn_RPC), RpcTarget.All, val, id);
    }

    public void SetPocketCards(CardData card1, CardData card2, int playerId)
    {
        int[] binaryData1 = card1.ConvertToBinary();
        int[] binaryData2 = card2.ConvertToBinary();
        
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
    private void EnableTurn_RPC(bool b, int _id)
    {
        if(IsLocalPlayer && id == _id)
            OnEnableTurn?.Invoke(b);
    }

    [PunRPC]
    private void SyncInformation(int id, string nickName)
    {
        this.nickName = nickName;
        this.id = id;
    }
    [PunRPC]
    private void SyncInformation(int credits)
    {
        this._totalCredit = credits;
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
    private void SetSelectedBetActionServerRpc(int playerAction, int _id)
    {
        print($"Checking : {id}");
        lastBetAction = (BetAction) playerAction;
        
        if(!PhotonNetwork.IsMasterClient)
            return;
     
        if(id == _id)
            Betting.PlayerEndBettingEvent?.Invoke(new BetActionInfo(this, lastBetAction, 0));
    }
    
    [PunRPC]
    private void SyncPocketCards(int[] binaryCardData1, int[] binaryCardData2, int playerID)
    {
        if(playerID != id)
            return;
        
        pocketCard1 = CardData.ConvertBinaryToCardData(binaryCardData1);
        pocketCard2 = CardData.ConvertBinaryToCardData(binaryCardData2);
    }
    
    [PunRPC]
    private void ShutdownClientRpc()
    {
        
    }
    
    #endregion


}
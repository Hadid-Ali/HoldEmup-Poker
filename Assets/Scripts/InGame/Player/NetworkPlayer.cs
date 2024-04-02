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
    [SerializeField] public Player PhotonPlayer;
    
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
        Invoke(nameof(OnNetworkSpawn), 1f);
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
    public void SetBetAction(BetAction betAction) => SetSelectedBetActionServerRpc((int) betAction);
    public void DealCards(NetworkPlayer player) => player.SetPocketCards(DecksHandler.GetRandomCard(),DecksHandler.GetRandomCard(), player.id);

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
        
        _canMakeTurn = false;
        OnEnableTurn.Invoke(false);
    }
    public void EnableTurn(bool val)
    {
        if(!IsLocalPlayer)
            return;
        
        _canMakeTurn = val;
        OnEnableTurn?.Invoke(val);
        
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
    private void SetSelectedBetActionServerRpc(int playerAction)
    {
        lastBetAction = (BetAction) playerAction;
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
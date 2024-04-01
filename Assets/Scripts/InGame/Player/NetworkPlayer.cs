using System;
using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public enum PlayerAction
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
    public PlayerAction playerAction;
    public int betAmount;
    
    public CardData pocketCard1;
    public CardData pocketCard2;

    private PlayerAction _selectedPlayerAction;
    public bool IsLocalPlayer => _photonView.IsMine;

    public static Action<NetworkPlayer> OnPlayerSpawn;


    private void Start()
    {
        Invoke(nameof(OnNetworkSpawn), 1f);
        GameEvents.NetworkGameplayEvents.ExposePocketCardsLocally.Register(ExposeCardsLocally);
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.ExposePocketCardsLocally.UnRegister(ExposeCardsLocally);
    }

    public void OnNetworkSpawn() => OnPlayerSpawn?.Invoke(this);
    public void ExposeCardsLocally() => _photonView.RPC(nameof(SyncInformation), RpcTarget.All);
    public void SyncInformationGlobally() => _photonView.RPC(nameof(SyncInformation), RpcTarget.All, id,nickName);
    public void SetBetAction(PlayerAction playerAction) => SetSelectedBetActionServerRpc(playerAction);
    public void DealCards(NetworkPlayer player) => player.SetPocketCards(DecksHandler.GetRandomCard(),DecksHandler.GetRandomCard(), player.id);


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
    private void SyncInformation()
    {
        if(!IsLocalPlayer)
            return;
        
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.Raise(pocketCard1,pocketCard2);
    }
    
    [PunRPC]
    private void SetBetInputFieldValueServerRpc(int value)
    {
        betAmount = value;
    }
    
    [PunRPC]
    private void SetSelectedBetActionServerRpc(PlayerAction playerAction)
    {
        _selectedPlayerAction = playerAction;
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
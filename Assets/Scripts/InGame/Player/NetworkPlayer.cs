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
    
    private const int NullSeatNumber = -1;
    
    public string nickName;
    public int id;
    public int seatNumber = NullSeatNumber;
    public PlayerAction playerAction;
    public int betAmount;
    
    public CardData pocketCard1;
    public CardData pocketCard2;

    private PlayerAction _selectedPlayerAction;
    private float _lastSeatActionTime;
    private const float SeatActionCooldownSeconds = 2f;

    public static Action<NetworkPlayer> OnPlayerSpawn;


    private void Start()
    {
        Invoke(nameof(OnNetworkSpawn),1f);
    }

    public void OnNetworkSpawn()
    {
        OnPlayerSpawn?.Invoke(this);
        // _photonView.RPC(nameof(SyncInformation), RpcTarget.Others);
    }
    
    public void SetBetAction(PlayerAction playerAction)
    {
        SetSelectedBetActionServerRpc(playerAction);
    }

    public void DealCards(NetworkPlayer player)
    {
        player.SetPocketCards(DecksHandler.GetRandomCard(),DecksHandler.GetRandomCard(), player.id);
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
    private void SyncInformation(string nickName)
    {
        this.nickName = nickName;
    }
    
    [PunRPC]
    private void SetSeatServerRpc(int _seatNumber)
    {
        seatNumber = _seatNumber;
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
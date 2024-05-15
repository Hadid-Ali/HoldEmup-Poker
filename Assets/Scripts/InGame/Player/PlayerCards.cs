using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerCards : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private NetworkPlayer player;
    
    private List<CardData> _pocketCards = new(Constants.Player.PocketCardLimit);

    private void Awake() => 
        GameEvents.NetworkGameplayEvents.OnShowDown.Register(SubmitCards);
    private void OnDestroy() => 
        GameEvents.NetworkGameplayEvents.OnShowDown.UnRegister(SubmitCards);
    private void SubmitCards() => 
        GameEvents.NetworkGameplayEvents.NetworkSubmitRequest.Raise(new NetworkDataObject(_pocketCards, player.id));
    
    public void SetPocketCards(CardData card1, CardData card2)
    {
        int[] binaryData1 = card1.ConvertToIntArray();
        int[] binaryData2 = card2.ConvertToIntArray();
        
        photonView.RPC(nameof(SyncPocketCards_RPC), RpcTarget.All, binaryData1,binaryData2);
    }

    public void ExposeCardsLocally() => 
        GameEvents.NetworkPlayerEvents.ExposePocketCardsLocally.Raise(_pocketCards);

    #region RPC
    
    [PunRPC]
    private void SyncPocketCards_RPC(int[] binaryCardData1, int[] binaryCardData2)
    {
        _pocketCards.Add(CardData.ConvertIntArrayToCardData(binaryCardData1));
        _pocketCards.Add(CardData.ConvertIntArrayToCardData(binaryCardData2));
    }

    [PunRPC]
    private void ExposePocketCards()
    {
        
    }
    
    #endregion
    
}

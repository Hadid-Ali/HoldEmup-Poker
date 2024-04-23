
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardCards : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    
    private List<CardData> _cards = new();
    private readonly List<int[]> _intCards = new();

    public List<CardData> GetCards => _cards;
    public void PopulateCards()
    {
        _cards = DecksHandler.GetRandomHand(5);

        foreach (var t in _cards)
            _intCards.Add(t.ConvertToIntArray());
    }

    
    public void ExposeCards(int length)
    {
        object[] objArray = new object[length];
        
        for (int i = 0; i < length; i++)
            objArray[i] = _intCards[i];
           
        photonView.RPC(nameof(SyncExposedCards), RpcTarget.All, objArray);
    }
    
    [PunRPC]
    private void SyncExposedCards(object[] objArray)
    {
        List<int[]> receivedList = objArray.Cast<int[]>().ToList();

        CardData[] cardData = new CardData[objArray.Length];
        
        for (int i = 0; i < cardData.Length; i++)
            cardData[i] = CardData.ConvertIntArrayToCardData(receivedList[i]);
        
        GameEvents.NetworkGameplayEvents.OnBoardCardsView.Raise(cardData);                         
    }

}
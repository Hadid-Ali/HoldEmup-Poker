using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkBotManager : MonoBehaviour
{
    [SerializeField] public List<CardData> cards = new();
    [SerializeField] private NetworkPlayerBotController _botController;
    [SerializeField] private PhotonView view;

    [SerializeField] private int cardsReceivedFromId;


    public void ReceiveHandData(CardData[] obj, int _ID)
    {
        if(_ID != _botController.ID)
            return;

        cardsReceivedFromId = _ID;
        cards.Clear();
        cards.AddRange(obj);
    }
    
    public List<CardData> GetCards()
    {
        return cards;
    }
    
}

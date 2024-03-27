using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkCardsDealer : MonoBehaviour
{
    [SerializeField] private DecksHandler m_DecksHandler;

    private int m_HandSize;

    private void Awake()
    {
        m_HandSize = (GameData.MetaData.DeckSize * GameData.MetaData.DecksCount) + GameData.MetaData.OffsetCards;
    }

    private void OnEnable()
    {
        GameEvents.NetworkEvents.PlayerReceiveCardsData.Register(ReceiveHandData);
    }

    private void OnDisable()
    {
        GameEvents.NetworkEvents.PlayerReceiveCardsData.UnRegister(ReceiveHandData);
    }

    private void ReceiveHandData(string data, int ID)
    {
        NetworkHandObject handObject = NetworkHandObject.DeSerialize(data);
        DealCardsToLocalPlayer(handObject.PlayerHand, ID);
        
        print($"User hands Recieved with ID {ID}");
    }

    
    [ContextMenu("DealCards")]
    public void DealCardsToLocalPlayer(int id)
    {
       // DealCardsToLocalPlayer(m_DecksHandler.GetRandomHand(m_HandSize), id);
    }


    public void DealCardsToLocalPlayer(CardData[] cardsData, int ID)
    {
        GameEvents.NetworkGameplayEvents.UserHandReceivedEvent.Raise(cardsData, ID);
    }
    
    public void DealCardsToNetworkPlayer(PlayerController playerController)
    {
       // NetworkHandObject handObject = new(m_DecksHandler.GetRandomHand(m_HandSize));
      //  string serializedHand = NetworkHandObject.Serialize(handObject);

      //  playerController.SubmitCardData(serializedHand);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCardsMediator : MonoBehaviour
{
    [SerializeField] private DraggableCard m_DraggableCard;
    private Card m_CardAtHand;

    private static CardData m_CardData;
    
    public static CardData CurrentData
    {
        get => m_CardData;

        private set
        {
            Debug.LogError("Set Card Data");
            m_CardData = value;
        }
    }

    private void OnEnable()
    {
        GameEvents.GameplayEvents.CardDragStartEvent.Register(OnCardDragStart);
        GameEvents.GameplayEvents.CardDropEvent.Register(OnCardDrop);
        GameEvents.GameplayEvents.CardReplacedEvent.Register(OnCardDataReplaced);
    }

    private void OnDisable()
    {
        GameEvents.GameplayEvents.CardDragStartEvent.UnRegister(OnCardDragStart);
        GameEvents.GameplayEvents.CardDropEvent.UnRegister(OnCardDrop);
        GameEvents.GameplayEvents.CardReplacedEvent.UnRegister(OnCardDataReplaced);
    }

    private void OnCardDataReplaced(CardData previousData)
    {
        if (m_CardAtHand == null)
            return;

    //    m_CardAtHand.SetData(previousData, true, false);
        m_CardAtHand.SetActiveStatus(true);
    }
    
    private void OnCardDragStart(CardData cardData,Card cardDataHolder)
    {
        CurrentData = cardData;
        m_CardAtHand = cardDataHolder;
        
        m_DraggableCard.EnableWithData(cardData);
    }

    private void OnCardDrop()
    {
        m_DraggableCard.SetEnabled(false);
        m_CardAtHand.SetActiveStatus(true);
        CurrentData = null;

        m_CardAtHand = null;
    }
}
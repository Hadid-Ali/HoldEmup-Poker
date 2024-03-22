using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardDragHandler : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler,IBeginDragHandler,IDragHandler
{
    [SerializeField] private Card m_Card;
    
    public void OnDrag(PointerEventData eventData)
    {
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameCardsMediator.CurrentData != null)
            return;

        Debug.LogError("Start Drag");
        m_Card.SetActiveStatus(false);
        GameEvents.GameplayEvents.CardDragStartEvent.Raise(m_Card.CardData, m_Card);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameCardsMediator.CurrentData == null)
            return;
        
        m_Card.SetData(GameCardsMediator.CurrentData, false, false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Card.RefreshCard();
    } 
    
    public void OnDrop(PointerEventData eventData)
    {
        GameEvents.GameplayEvents.CardReplacedEvent.Raise(m_Card.CardData);
        m_Card.SaveData();
    }
}

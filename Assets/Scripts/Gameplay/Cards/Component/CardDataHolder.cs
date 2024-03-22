using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CardDataHolder : MonoBehaviour
{
    private CardData m_TempData;
    
    private GameEvent m_OnCardClear = new();
    private GameEvent m_OnCardDataSet = new();
    private GameEvent<CardData> m_SetCardDataInternal = new();
    
    
    public CardData CardData { get; private set; }

    public void Initialize(Action onCardClear, Action onCardDataSetPersistent,Action<CardData> onSetCardData)
    {
        m_OnCardClear.Register(onCardClear);
        m_OnCardDataSet.Register(onCardDataSetPersistent);
        m_SetCardDataInternal.Register(onSetCardData);
    }

    public void SetCardData(CardData cardData, bool isPersistent)
    {
        SetCardDataInternal(cardData);

        m_TempData = cardData;

        if (!isPersistent)
            return;
        
        SaveData();
    }

    public void SaveData()
    {
        CardData = m_TempData;
        m_OnCardDataSet.Raise();
    }

    public void RefreshData()
    {
        if (CardData == null)
            return;
        
        SetCardDataInternal(CardData);
    }

    public void ClearCardData()
    {
        m_OnCardClear.Raise();
    }

    private void SetCardDataInternal(CardData cardData)
    {
        m_SetCardDataInternal.Raise(cardData);
    }
}

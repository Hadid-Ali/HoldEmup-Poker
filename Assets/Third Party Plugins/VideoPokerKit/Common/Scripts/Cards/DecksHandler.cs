using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DecksHandler : MonoBehaviour
{
    [Header("The deck"),SerializeField] private List<CardData> m_CardsRegistry;
    
    private int m_CurrentNumberOfDecksToUse = 1;
    private List<CardData> m_CurrentGameDeck = new();
    
    private void OnEnable()
    {
        GameEvents.GameFlowEvents.RoundStart.Register(LoadInternalDecks);
    }

    private void OnDisable()
    {
        GameEvents.GameFlowEvents.RoundStart.UnRegister(LoadInternalDecks);
    }

    private void LoadInternalDecks()
    {
        SetDecksCount(m_CurrentNumberOfDecksToUse);
    }

    private CardData GetRandomCard()
    {
        int index = Random.Range(0, m_CurrentGameDeck.Count);
        
        if (index >= m_CurrentGameDeck.Count)
            return null;
        
        CardData data = m_CurrentGameDeck[index];
        m_CurrentGameDeck.RemoveAt(index);

        return data;
    }
    
    public void SetDecksCount(int count)
    {
        m_CurrentNumberOfDecksToUse = count;
        SetupCurrentGameDeck();
    }
    
    void SetupCurrentGameDeck()
    {
        m_CurrentGameDeck.Clear();        
        for (int i = 0; i < m_CurrentNumberOfDecksToUse; i++)
        {
            m_CurrentGameDeck.AddRange(m_CardsRegistry);
        }
        
        Debug.LogError($"Loaded Decks{m_CurrentGameDeck.Count}");
    }

    public CardData[] GetRandomHand(int cardsAmount)
    {
        List<CardData> handsData = new();

        for (int i = 0; i < cardsAmount; i++)
        {
            handsData.Add( GetRandomCard());
        }

        Debug.LogError($"Counter After {m_CurrentGameDeck.Count}");
        return handsData.ToArray();
    }
}

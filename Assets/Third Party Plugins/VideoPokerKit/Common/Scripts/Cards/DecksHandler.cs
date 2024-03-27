using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DecksHandler : MonoBehaviour
{
    [Header("The deck"),SerializeField] private List<CardData> m_CardsRegistry;
    
    private int m_CurrentNumberOfDecksToUse = 2;
    private static List<CardData> m_CurrentGameDeck = new();

    private void Start()
    {
        m_CurrentGameDeck.Clear();        
        for (int i = 0; i < m_CurrentNumberOfDecksToUse; i++)
        {
            m_CurrentGameDeck.AddRange(m_CardsRegistry);
        }
    }

    public static CardData GetRandomCard()
    {
        int index = Random.Range(0, m_CurrentGameDeck.Count);
        
        if (index >= m_CurrentGameDeck.Count)
            return null;
        
        CardData data = m_CurrentGameDeck[index];
        m_CurrentGameDeck.RemoveAt(index);

        return data;
    }
    public static List<CardData> GetRandomHand(int cardsAmount)
    {
        List<CardData> handsData = new();

        for (int i = 0; i < cardsAmount; i++)
        {
            handsData.Add( GetRandomCard());
        }
        
        return handsData;
    }
}

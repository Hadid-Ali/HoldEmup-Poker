using System;
using System.Collections.Generic;
using UnityEngine;

public class GameCardsData : SceneBasedSingleton<GameCardsData>
{
    [SerializeField] private Transform m_DecksContainer;

    public List<CardData> GetDecksData()
    {
        List<CardsDeck> decks = new();
        List<CardData> data = new();
      
        int childCount = m_DecksContainer.childCount;
      
        for (int i = 0; i < childCount; i++)
        {
            decks.Add(m_DecksContainer.GetChild(i).GetComponent<CardsDeck>());
        }
        
        decks.ForEach(deck => data.AddRange(deck.CardsData));
        return data;
    }
}

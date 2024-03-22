using System;
using UnityEngine;

public class CardsDeck : MonoBehaviour
{
   [SerializeField] private DeckName m_DeckName;
   [SerializeField] private Card[] m_Cards;

   private int m_CardsCount = 0;
   
   public CardData[] CardsData
   {
      get
      {
         CardData[] cards = new CardData[m_Cards.Length];

         for (int i = 0; i < m_Cards.Length; i++)
         {
            cards[i] = m_Cards[i].CardData;
         }

         return cards;
      }
   }

   private void Awake()
   {
      for (int i = 0; i < m_Cards.Length; i++)
      {
         m_Cards[i].InitializeWithAction(OnDeckCardUpdated);
      }
   }

   private void OnEnable()
   {
      GameEvents.GameplayUIEvents.EvaluateDeck.Register(EvaluateDeckInternal);
   }

   private void OnDisable()
   {
      GameEvents.GameplayUIEvents.EvaluateDeck.UnRegister(EvaluateDeckInternal);
   }

   public void PrintDeck()
   {
      Array.ForEach(CardsData, card => Debug.LogError($"Data : {card}"));
   }

   private void OnDeckCardUpdated()
   {
      if (m_CardsCount < GameData.MetaData.DeckSize)
      {
         m_CardsCount++;  
      }

      if (m_CardsCount >= GameData.MetaData.DeckSize)
      {
         EvaluateDeckInternal();
      }
   }

   private void EvaluateDeckInternal()
   {
      HandTypes handType = CardsManager.EvaluateDeck(CardsData);
      GameEvents.GameplayEvents.CardDeckUpdated.Raise(m_DeckName, handType);  
   }
}

using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class CardsManager : MonoBehaviour
{
	[SerializeField] private DecksHandler m_DecksHandler;
	// screen cards
	[Header("Player cards"),SerializeField] private Card [] m_GameCards;

	[SerializeField] private GameplayHandTypeEvent m_HandTypeEvent;

	// temporary cards used for evaluating hand
	CardData [] workCards;

	
	//--------------------------------------------------------

	// Use this for initialization
	void Start () 
	{
		workCards = new CardData[5];
		for(int i=0; i<5; i++)
			workCards[i] = new CardData();
	}




	private void OnEnable()
	{
		GameEvents.GameplayUIEvents.CardsArrangementUpdated.Register(OnCardsArrangementUpdated);
		GameEvents.NetworkGameplayEvents.UserHandReceivedEvent.Register(DealCards);
		GameEvents.GameplayEvents.CardReplacedEvent.Register(OnCardDataReplaced);
	}

	private void OnDisable()
	{
		GameEvents.GameplayUIEvents.CardsArrangementUpdated.UnRegister(OnCardsArrangementUpdated);
		GameEvents.NetworkGameplayEvents.UserHandReceivedEvent.UnRegister(DealCards);
		GameEvents.GameplayEvents.CardReplacedEvent.UnRegister(OnCardDataReplaced);
	}

	private void OnCardDataReplaced(CardData previousData)
	{
		CardData c = GameCardsMediator.CurrentData;
		Debug.LogError($"Replaced {previousData.value} {previousData.type} with {c.value} {c.type}");

		Card cardA = Array.Find(m_GameCards,
			card => card.CardData.type == previousData.type && card.CardData.value == previousData.value);
		Card cardB = Array.Find(m_GameCards, card => card.CardData.type == c.type && card.CardData.value == c.value);
		
		cardA.SetData(c, true, false);
		cardB.SetData(previousData, true, false);
	}
	
	private void OnCardsArrangementUpdated(Card[] cards)
	{
		m_GameCards = null;
		m_GameCards = cards;
	}
	
	public void ClearHand()
	{
		// make cards vanish
		for (int i=0; i<5; i++) 			
			m_GameCards [i].ClearAfterDeal();
	}

	//--------------------------------------------------------

	public void ResetDeck()
	{
		// mark all cards as not dealt
	//	for(int i = 0; i<m_CardsRegistry.Count; i++)
	//		m_CardsRegistry[i].dealt = false;
	}
	
	//--------------------------------------------------------
	
	public void DealCards(CardData[] cardsData, int ID)
	{
		for (int i = 0; i < m_GameCards.Length; i++)
		{
			m_GameCards[i].SetData(cardsData[i], true, true);
		}
	}

	public static HandTypes EvaluateDeck(CardData[] cards)
	{
		// evaluate the temp hand
		HandEvaluator.Evaluate(cards, out HandTypes handType); // show wins only in the RESULTS stage
		return handType;
	}

	public void EvaluateHand()
	{
		// copy cards into a separate array of cards
		// (they will be sorted and better not mess up original cards)
		for(int i=0; i<5; i++)			
			workCards[i].CopyInfoFrom( m_GameCards[i].CardData );

		// evaluate the temp hand
		HandEvaluator.Evaluate(workCards,out HandTypes handType); // show wins only in the RESULTS stage

		bool cardsWereHolded = false;
		m_HandTypeEvent.Raise(handType);

	}

	//--------------------------------------------------------
}

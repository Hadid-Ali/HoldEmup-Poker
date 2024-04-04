using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardCardsView : MonoBehaviour
{
    [SerializeField] private Image[] boardCards;
    public void ExposeCards(CardData[] cards)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            boardCards[i].sprite = CardsRegistery.Instance.GetCardSprite(cards[i].type, cards[i].value);
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class BoardCardsView : MonoBehaviour
{
    [SerializeField] private Image[] boardCards;

    private void Awake()
    {
        GameEvents.NetworkGameplayEvents.OnBoardCardsView.Register(ExposeCards);
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.OnBoardCardsView.UnRegister(ExposeCards);
    }

    public void ExposeCards(CardData[] cards)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            boardCards[i].sprite = CardsRegistery.Instance.GetCardSprite(cards[i].type, cards[i].value);
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class BoardCardsView : MonoBehaviour
{
    [SerializeField] private Image[] boardCards;
    [SerializeField] private Sprite defaultCardBack;

    private void Awake()
    {
        GameEvents.NetworkGameplayEvents.OnBoardCardsView.Register(ExposeCards);
        GameEvents.NetworkGameplayEvents.OnBoardCardsViewReset.Register(ResetView);
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.OnBoardCardsView.UnRegister(ExposeCards);
        GameEvents.NetworkGameplayEvents.OnBoardCardsViewReset.UnRegister(ResetView);
    }

    public void ResetView()
    {
        foreach (var t in boardCards)
            t.sprite = defaultCardBack;
    }

    public void ExposeCards(CardData[] cards)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            boardCards[i].sprite = CardsRegistery.Instance.GetCardSprite(cards[i].type, cards[i].value);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsRegistery : MonobehaviourSingleton<CardsRegistery>
{
    [SerializeField] private List<CardDataObject> m_Cards = new();

    public Sprite GetCardSprite(CardType cardType, CardValue cardValue) =>
        m_Cards.Find(card => card.type == cardType && card.value == cardValue).CardImage;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardsHandData
{
    public CardData[] Cards;
    public Guid HandID;

    public CardsHandData(CardData[] cards)
    {
        Cards = cards;
        HandID = Guid.NewGuid();
    }
}

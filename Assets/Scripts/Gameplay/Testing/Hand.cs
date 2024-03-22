using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    public CardData[] CardData;
    public HandTypes _HandType;
    public int photonID;

    public Hand(CardData[] cardData, HandTypes _handType, int _photonID)
    {
        CardData = cardData;
        _HandType = _handType;
        photonID = _photonID;
    }
}

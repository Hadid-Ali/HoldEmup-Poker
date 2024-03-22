using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDecksObject
{
    public int userID;
    public List<CardData[]> Decks = new();
}

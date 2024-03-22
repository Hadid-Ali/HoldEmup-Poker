using System;
using UnityEngine;

[Serializable]
public class DeckHandStringObject
{
    [field: SerializeField] public HandTypes HandType { get; private set; }
    [field: SerializeField] public string HandTypeIdentifier { get; private set; }
}

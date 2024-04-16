using System.Collections.Generic;
using UnityEngine;

public class DeckHandsRegistry : MonobehaviourSingleton<DeckHandsRegistry>
{
    [SerializeField] private List<DeckHandStringObject> m_DeckIdentifier = new();

    public string GetHandTypeName(HandTypes handType) =>
        m_DeckIdentifier.Find(x => x.HandType == handType).HandTypeIdentifier;
}

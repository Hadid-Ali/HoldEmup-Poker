using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecksContainer : MonoBehaviour
{
    private void Start()
    {
        SetDeckNames();
    }

    private void OnEnable()
    {
        GameEvents.GameplayUIEvents.DeckArrangementUpdated.Register(SetDeckNames);
    }

    private void OnDisable()
    {
        GameEvents.GameplayUIEvents.DeckArrangementUpdated.UnRegister(SetDeckNames);
    }

    private void SetDeckNames()
    {
        CardsDeckViewHandler[] decks = GetComponentsInChildren<CardsDeckViewHandler>();

        for (int i = 0; i < decks.Length; i++)
        {
            decks[i].SetIndex(i + 1);
        }
    }
}

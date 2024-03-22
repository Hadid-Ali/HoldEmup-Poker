using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderCardsContainer : MonoBehaviour
{
    [SerializeField] private GameObject m_CardsContainer;

    private void OnEnable()
    {
        GameEvents.GameplayEvents.GameplayCardsStateChanged.Register(OnGameplayCardsStateChanged);
    }

    private void OnDisable()
    {
        GameEvents.GameplayEvents.GameplayCardsStateChanged.UnRegister(OnGameplayCardsStateChanged);
    }

    private void OnGameplayCardsStateChanged(bool status)
    {
        m_CardsContainer.SetActive(!status);
    }
}

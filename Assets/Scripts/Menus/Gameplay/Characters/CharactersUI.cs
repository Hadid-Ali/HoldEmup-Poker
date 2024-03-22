using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class CharactersUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro m_CharacterNameText;

    [SerializeField] private StringEvent m_NamedUpdatedEvent;

    private void OnEnable()
    {
        m_NamedUpdatedEvent.Register(OnNameUpdated);
    }

    private void OnDisable()
    {
        m_NamedUpdatedEvent.Unregister(OnNameUpdated);
    }

    void OnNameUpdated(string playerName)
    {
        m_CharacterNameText.text = playerName;
    }
}

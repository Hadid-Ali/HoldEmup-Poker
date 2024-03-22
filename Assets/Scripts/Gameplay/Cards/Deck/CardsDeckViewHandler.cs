using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardsDeckViewHandler : MonoBehaviour
{
    [SerializeField] private Image m_DeckImageComponent;
    [SerializeField] private TextMeshProUGUI m_DeckNameText;

    [SerializeField] private Color m_DeckFocusColor;
    [SerializeField] private Color m_DeckNormalColor;

    public void SetIndex(int index)
    {
        m_DeckNameText.text = $"ROW {index}";
    }
    
    public void SetFocused(bool status)
    {
        m_DeckImageComponent.color = status ? m_DeckFocusColor : m_DeckNormalColor;
    }
}
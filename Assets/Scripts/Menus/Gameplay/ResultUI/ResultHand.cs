using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultHand : MonoBehaviour
{
    [Header("Refs")]
    
    [SerializeField] private ResultDeck m_CardsDeck;
    
    [SerializeField] private GameObject m_HandObjectContainer;
    [SerializeField] private GameObject m_LoserHandOverlay;
    [SerializeField] private GameObject m_WinnerNameOverlay;
    
    [Header("Texts")]
    
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_ScoreText;
    [SerializeField] private TextMeshProUGUI m_HandNameText;
    
    private bool m_IsWinnerDeck = false;

    private void Start()
    {
        ResetInternal();
    }

    public void SetDecksContainer(ResultHandDataObject resultHandDataObject)
    {
        ResetInternal();
        m_IsWinnerDeck = resultHandDataObject.IsWinner;

        m_ScoreText.text = m_IsWinnerDeck ? $"+{resultHandDataObject.Score}" : "0";
        m_NameText.text = resultHandDataObject.PlayerName;
        m_HandNameText.text = resultHandDataObject.HandName;
        
        m_CardsDeck.SetHandData(resultHandDataObject.Cards);
        SetVisibilityStatus(true);

        Invoke(nameof(CheckForWinStatus), resultHandDataObject.WinnerRevealDuration);
    }

    public void Reset()
    {
        ResetInternal();
        SetVisibilityStatus(false);
    }
    
    private void ResetInternal()
    {
        SetLoserOverlayStatus(false);
        SetTextVisibilityStatus(false);
        SetWinnerNameOverlayStatus(false);
    }

    private void CheckForWinStatus()
    {
        SetTextVisibilityStatus(m_IsWinnerDeck);
        SetWinnerNameOverlayStatus(m_IsWinnerDeck);
        SetLoserOverlayStatus(!m_IsWinnerDeck);
    }
    
    private void SetVisibilityStatus(bool status)
    {
        m_HandObjectContainer.SetActive(status);
    }

    private void SetTextVisibilityStatus(bool status)
    {
        m_ScoreText.gameObject.SetActive(status);
    }
    
    private void SetLoserOverlayStatus(bool status)
    {
        m_LoserHandOverlay.SetActive(status);
    }

    private void SetWinnerNameOverlayStatus(bool status)
    {
        m_WinnerNameOverlay.SetActive(status);
    }
}

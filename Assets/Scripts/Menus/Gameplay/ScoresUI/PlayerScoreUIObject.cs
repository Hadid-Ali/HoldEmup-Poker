using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreUIObject : MonoBehaviour
{
    [Header("Components")]
    
    [SerializeField] private List<Sprite> m_WinnerIcons;
    [SerializeField] private Image m_ImageComponent;
    
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_ScoreText;

    [Header("Attributes")]
    
    [SerializeField] private int m_PositionIndex;
    public int PositionIndex => m_PositionIndex;

    private void OnEnable()
    {
        GameEvents.NetworkGameplayEvents.MatchWinnersAnnounced.Register(OnWinnerAnnounced);
        GameEvents.GameplayEvents.PlayerPositionInit.Register(OnPlayerPositionInit);
    }

    private void OnDisable()
    {
        GameEvents.NetworkGameplayEvents.MatchWinnersAnnounced.UnRegister(OnWinnerAnnounced);
        GameEvents.GameplayEvents.PlayerPositionInit.UnRegister(OnPlayerPositionInit);
    }

    private void OnPlayerPositionInit(int index, Vector3 position)
    {
        if (m_PositionIndex != index)
            return;

        transform.position = Dependencies.GameplayRefs.GetWorldToScreenPoint(position);
    }

    private void OnWinnerAnnounced(List<int> winnerNetworkIDs, bool isWinner)
    {
        List<int> localIDs = new()
        {
            Dependencies.PlayersContainer.GetPlayerLocalID(winnerNetworkIDs[0]),
            Dependencies.PlayersContainer.GetPlayerLocalID(winnerNetworkIDs[1]),
        };

        if (winnerNetworkIDs.Count > 2)
        {
            localIDs.Add(Dependencies.PlayersContainer.GetPlayerLocalID(winnerNetworkIDs[2]));
        }

        if (!localIDs.Contains(m_PositionIndex))
            return;

        SetImageSprite(m_WinnerIcons[localIDs.FindIndex(id => id == m_PositionIndex)]);
    }

    private void SetImageSprite(Sprite sprite)
    {
        m_ImageComponent.sprite = sprite;
        m_ImageComponent.gameObject.SetActive(true);
    }

    public void SetContainerStatus(bool activeState)
    {
        gameObject.SetActive(activeState);
    }
    
    public void SetName(string text)
    {
        m_NameText.text = text;
    }

    public void SetScore(int score)
    {
        m_ScoreText.text = $"{score.ToString()} pts";
    }
}

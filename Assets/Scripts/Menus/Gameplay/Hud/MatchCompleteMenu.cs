using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchCompleteMenu : UIMenuBase
{
    [Header("UI Components"),Space(5)]
    
    [SerializeField] private List<ScoreWidget> m_ScoreWidgets = new();
    
    [SerializeField] private TextMeshProUGUI m_MatchCompleteTitleText;
    [SerializeField] private TextMeshProUGUI m_MatchCompleteText;

    [Header("Attributes"),Space(5)]
    
    [SerializeField] private string m_WinnerMessage = "Congratulations! You won the game";
    [SerializeField] private string m_LoserMessage = "Sorry you did not win this game";

    [SerializeField] private string m_WinnerTitle = "Winner!!";
    [SerializeField] private string m_LoserTitle = "Match Completed";

    
    [SerializeField] private float m_WaitBeforeMenuTransition = 3.5f;
    
    private void OnEnable()
    {
       // GameEvents.GameplayUIEvents.DispatchScores.Register(SetupScores);
        GameEvents.NetworkGameplayEvents.MatchWinnersAnnounced.Register(OnMatchWinnerAnnounced);
    }

    private void OnDisable()
    {
       // GameEvents.GameplayUIEvents.DispatchScores.UnRegister(SetupScores);
        GameEvents.NetworkGameplayEvents.MatchWinnersAnnounced.UnRegister(OnMatchWinnerAnnounced);
    }

    protected override void OnContainerEnable()
    {
        base.OnContainerEnable();
        GameEvents.GameplayUIEvents.MatchCompleteMenuShown.Raise();
        Invoke(nameof(DisableInternal), m_WaitBeforeMenuTransition);
    }

    private void DisableInternal()
    {
        ChangeMenuState(MenuName.GameOverMenu);
    }

    private void OnMatchWinnerAnnounced(List<int> networkIDs,bool isWinner)
    {
        m_MatchCompleteTitleText.text = isWinner ? m_WinnerTitle : m_LoserTitle;
        m_MatchCompleteText.text = isWinner ? m_WinnerMessage : m_LoserMessage;
    }
    
    private void SetupScores( List<KeyValuePair<int, int>> scores)
    {
        for (int i = 0; i < scores.Count(); i++)
        {
            SetScoreOnWidget(scores[i], m_ScoreWidgets[i]);
        }
    }

    private void SetScoreOnWidget(KeyValuePair<int, int> scorePair, ScoreWidget widget)
    {
        string name = Dependencies.PlayersContainer.GetPlayerName(scorePair.Key);
        widget.SetScore($"{name} : {scorePair.Value} pts");
    }

}

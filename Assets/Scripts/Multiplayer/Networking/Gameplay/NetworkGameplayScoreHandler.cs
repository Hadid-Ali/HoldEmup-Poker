using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class NetworkGameplayScoreHandler : MonoBehaviour
{
    private Dictionary<int,int> m_PlayerScoreObjects = new();
    private GameEvent<int, int, int> m_OnPlayerWin = new();

    private int m_RecievedScores = 0;
    
    private void OnEnable()
    {
        GameEvents.GameplayEvents.RoundCompleted.Register(OnRoundCompleted);
        GameEvents.GameFlowEvents.RoundStart.Register(OnRoundStart);
        GameEvents.GameFlowEvents.MatchOver.Register(OnMatchOver);
        
        GameEvents.GameplayEvents.OnPlayerScoreSubmit.Register(SyncNetworkPlayerScoreReceived_RPC);
    }

    private void OnDisable()
    {
        GameEvents.GameplayEvents.RoundCompleted.UnRegister(OnRoundCompleted);
        GameEvents.GameFlowEvents.RoundStart.UnRegister(OnRoundStart);
        GameEvents.GameFlowEvents.MatchOver.UnRegister(OnMatchOver);

        GameEvents.GameplayEvents.OnPlayerScoreSubmit.UnRegister(SyncNetworkPlayerScoreReceived_RPC);
    }

    public void Initialize(Action<int, int, int> onPlayerWin)
    {
        m_OnPlayerWin.Register(onPlayerWin);
    }

    private void OnMatchOver()
    {
        GameData.RuntimeData.ResetTotalPlayerScore();
        GameData.RuntimeData.ResetBotSpawnCount();
    }
    
    private void OnRoundStart()
    {
        m_RecievedScores = 0;
    }

    public int GetUserScore(int playerID) => m_PlayerScoreObjects[playerID];

    // public virtual void SyncNetworkScoreObjectOverNetwork()
    // {
    //     int totalScore = GameData.RuntimeData.TOTAL_PLAYER_SCORE;
    //     int id = Dependencies.PlayersContainer.GetLocalPlayerNetworkID();
    //
    //
    //     NetworkManager.NetworkUtilities.RaiseRPC(m_NetworkGameplayHandler.NetworkViewComponent,
    //         nameof(SyncNetworkPlayerScoreReceived_RPC), RpcTarget.All, new object[]
    //         {
    //             id,
    //             totalScore
    //         });
    //     
    //
    // }

    // [PunRPC]
    public void SyncNetworkPlayerScoreReceived_RPC(int photonViewID, int score)
    {
        m_PlayerScoreObjects[photonViewID] = score;
        
        m_RecievedScores++;
        
        if(!PhotonNetwork.IsMasterClient)
            return;
        
        if (m_RecievedScores >= GameData.SessionData.CurrentRoomPlayersCount)
            CheckForWinner();
    }

    //TODO: Implement Tie Breaker
    private void CheckForWinner()
    {
        
        IOrderedEnumerable<KeyValuePair<int, int>> scoresOrderedByDescending = m_PlayerScoreObjects.OrderByDescending(pair => pair.Value);
        IEnumerable<KeyValuePair<int, int>> entries = m_PlayerScoreObjects.Where(pair => pair.Value >= GameData.MetaData.TotalScoreToWin);
        
        List<KeyValuePair<int, int>> keyValuePairs = entries.ToList();

        if (!keyValuePairs.Any())
            return;

        keyValuePairs = scoresOrderedByDescending.ToList();
        
        int highestScore = keyValuePairs.FirstOrDefault().Value;
        List<KeyValuePair<int, int>> highestKVPs = keyValuePairs.FindAll(pair => pair.Value == highestScore);

        if (highestKVPs.Count > 1)
        {
            return;
        }

        int thirdID = keyValuePairs.Count > 2 ? keyValuePairs[2].Key : GameData.MetaData.NullID;
        m_OnPlayerWin.Raise(highestKVPs.First().Key, keyValuePairs[1].Key, thirdID);
        print($"Winner Checking : {highestKVPs.First().Key}");
        DispatchSortedScores();
    }

    private void OnRoundCompleted()
    {
        //SyncNetworkScoreObjectOverNetwork();
    }

    private void DispatchSortedScores()
    {
        List<KeyValuePair<int, int>> sortedScores = m_PlayerScoreObjects.ToList();
        sortedScores.OrderByDescending(pair => pair.Value);
        
        GameEvents.GameplayUIEvents.DispatchScores.Raise(sortedScores);
    }
}

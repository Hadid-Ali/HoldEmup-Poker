using UnityEngine;
using Photon.Pun;

public class NetworkMatchStartHandler : MonoBehaviour
{
    private bool m_IsAutoStartRequestSent = false;
    private int m_MaxPlayers = 0;

    private int CurrentPlayersCount => PhotonNetwork.PlayerList.Length;
    
    public void SetMaxPlayersCount(int count)
    {
        m_MaxPlayers = count;
    }
    
    public void OnPlayerEnteredInRoom()
    {
        CheckForMinimumPlayersCount();
        CheckForMaximumPlayersCount();
        print("Player Entered room");
    }

    private void CheckForMinimumPlayersCount()
    {
        if (m_IsAutoStartRequestSent)
            return;
        
        if (CurrentPlayersCount >= GameData.MetaData.MinimumRequiredPlayers)
        {
            print("Match could be started");
            
            GameEvents.TimerEvents.ExecuteActionRequest.Raise(new TimerDataObject()
            {
                Title = "Starting The Match",
                TimeDuration = GameData.MetaData.WaitBeforeAutomaticMatchStart,
                ActionToExecute =  StartMatchInternal,
                IsNetworkGlobal = true
            });
            m_IsAutoStartRequestSent = true;
            
            print("Match started");
        }
    }

    private void CheckForMaximumPlayersCount()
    {
        if (CurrentPlayersCount >= m_MaxPlayers)
        {
            TerminateAutoMatchStartRequest();
            StartMatchInternal();
        }
    }

    public void OnPlayerLeftRoom()
    {
        if (CurrentPlayersCount < GameData.MetaData.MinimumRequiredPlayers)
            TerminateAutoMatchStartRequest();
    }

    private void TerminateAutoMatchStartRequest()
    {
        GameEvents.TimerEvents.CancelActionRequest.Raise();
        m_IsAutoStartRequestSent = false;
    }

    public void StartMatchInternal()
    {
        GameData.SessionData.CurrentRoomPlayersCount = CurrentPlayersCount;
        GameEvents.NetworkEvents.PlayersJoined.Raise();
        PhotonNetwork.CurrentRoom.IsOpen = false;
        m_IsAutoStartRequestSent = false;
    }
}

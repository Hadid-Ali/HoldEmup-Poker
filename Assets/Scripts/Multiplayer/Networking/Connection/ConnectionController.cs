using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ConnectionController : MonoBehaviourPunCallbacks
{
    [SerializeField] private NetworkSceneManager sceneManager;
    private RegionHandler m_RegionHandler;
    
    private bool m_IsTestConnection = true;
    private bool m_CanReconnect = false;

    public override void OnEnable()
    {
        base.OnEnable();
        GameEvents.GameFlowEvents.RoundStart.Register(OnRoundStart);
        GameEvents.GameFlowEvents.MatchOver.Register(OnMatchOver);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        GameEvents.GameFlowEvents.RoundStart.UnRegister(OnRoundStart);
        GameEvents.GameFlowEvents.MatchOver.UnRegister(OnMatchOver);
    }

    private void OnRoundStart()
    {
        m_CanReconnect = true;
    }

    private void OnMatchOver()
    {
        m_CanReconnect = false;
        m_IsTestConnection = true;
        GameData.SessionData.CurrentRoomPlayersCount = 0;
    }
    
    private void UpdateConnectionStatus(string status)
    {
        GameEvents.MenuEvents.NetworkStatusUpdated.Raise(status);
    }

    private void OnDestroy()
    {
        PhotonNetwork.Disconnect();
    }

    public void StartConnectionWithName(string name)
    {
        PhotonNetwork.LocalPlayer.NickName = name;
        ConnectToServer();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        if (!m_CanReconnect)
            return;
        
        Debug.LogError($"{cause}");
        GameEvents.NetworkEvents.NetworkDisconnectedEvent.Raise();
        
        PhotonNetwork.ReconnectAndRejoin();
    }

    private void ConnectToServer()
    {
        UpdateConnectionStatus("\t\tConnecting");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogError("Connected to Master");
        if (m_IsTestConnection)
        {
            UpdateConnectionStatus("Finding Best Regions to Connect");
            Invoke(nameof(OnRegionsPingCompleted), 1f);
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnRegionListReceived(RegionHandler regionHandler)
    {
        m_RegionHandler = regionHandler;
    }

    private void OnRegionsPingCompleted()
    {
        List<Region> regions = m_RegionHandler.EnabledRegions;
        GameEvents.NetworkEvents.OnServerConnected.Raise(new RegionConfig()
        {
            Availableregions = regions,
            BestRegion = m_RegionHandler.BestRegion
        });
    }

    public void OnRegionSelect(Region region)
    {
        Disconnect();
        m_IsTestConnection = false;
        string regionCode = region.Code;
        PhotonNetwork.ConnectToRegion(regionCode);
        
        UpdateConnectionStatus(
            $"Connected To {NetworkManager.Instance.RegionsRegistry.GetRegionName(regionCode)}, Finding Lobby");
    }

    public void CreateRoom(RoomOptions roomOptions)
    {
        PhotonNetwork.JoinOrCreateRoom(Guid.NewGuid().ToString(), roomOptions, TypedLobby.Default);
        StartCoroutine(NotifyPlayerJoined_Routine());
    }

    IEnumerator NotifyPlayerJoined_Routine()
    {
        yield return new WaitForSeconds(GameData.MetaData.WaitBeforePlayerJoinNotify);
    }

    public virtual void OnCreateRoomFailed(short returnCode, string message)
    {
    }    
    
    public override void OnJoinedLobby()
    {
        UpdateConnectionStatus("\t\tFinding Match");
        PhotonNetwork.JoinRandomRoom();
        
        print("Joined Lobby");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        
        PhotonNetwork.CreateRoom(null, new RoomOptions());
        UpdateConnectionStatus("\t Setting Up Game");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        UpdateConnectionStatus($"\t Waiting For Others");
        
        GameEvents.NetworkEvents.OnRoomJoined.Raise();
        print("Joined Room");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        GameEvents.NetworkEvents.OnPlayerJoined.Raise();
    }
    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        GameEvents.NetworkEvents.OnPlayerJoined.Raise();
    }
    


    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void StartMatch()
    {
        sceneManager.LoadGameplayScene();
        print("Start Match");
        //m_MatchStartHandler.StartMatchInternal();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ConnectionController : MonoBehaviourPunCallbacks
{
    [SerializeField] private ConnectionControllerView m_ControllerView;
    [SerializeField] private int m_PlayersCount = 2;
    [SerializeField] private PhotonView m_PhotonView;

    private TypedLobby customLobby = new("gameLobby", LobbyType.SqlLobby);
    string sqlLobbyFilter = "C0 = '1'";
    
    private RegionHandler m_RegionHandler;

    private int m_RequiredPlayersCount = 2;
    private List<RoomInfo> m_Rooms = new();
    
    private bool m_IsTestConnection = true;
    private bool m_IsJoiningRoom = false;

    private void Start()
    {
        m_ControllerView.Initialize(StartConnectionWithName, OnRegionSelect, CreateRoom, OnCheckForRoomJoining);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        GameEvents.MenuEvents.MatchStartRequested.Register(StartMatchTimer);
        GameEvents.MenuEvents.RoomJoinRequested.Register(OnRoomJoinRequested);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        GameEvents.MenuEvents.MatchStartRequested.UnRegister(StartMatchTimer);
        GameEvents.MenuEvents.RoomJoinRequested.UnRegister(OnRoomJoinRequested);
    }

    private void OnCheckForRoomJoining()
    {
        m_IsJoiningRoom = true;
        
        TypedLobby typedLobby = new TypedLobby(PhotonNetwork.CurrentLobby.Name, LobbyType.SqlLobby);
        PhotonNetwork.GetCustomRoomList(typedLobby, sqlLobbyFilter);
    }
    
    private void OnRoomJoinRequested(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        UpdateConnectionStatus("Joining Room...");
    }
    
    private void UpdateConnectionStatus(string status)
    {
        NetworkManager.Instance.SetStatus(status);
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
        GameEvents.NetworkPlayerEvents.OnPlayerDisconnected.Raise();
        Debug.LogError($"{cause}");
    }

    private void ConnectToServer()
    {
        UpdateConnectionStatus("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
        
        print("Connected To  Server");
    }

    public override void OnConnectedToMaster()
    {
        if (m_IsTestConnection)
        {
            UpdateConnectionStatus("Connected to Server, Finding Best Regions to Connect");
            Invoke(nameof(OnRegionsPingCompleted), 1f);
        }
        else
        {
            PhotonNetwork.JoinLobby(customLobby);
        }
        print("Connected To Master Server");
    }

    public override void OnRegionListReceived(RegionHandler regionHandler)
    {
        m_RegionHandler = regionHandler;
    }
    
    private void OnRegionsPingCompleted()
    {
        List<Region> regions = m_RegionHandler.EnabledRegions;
        GameEvents.NetworkEvents.ConnectionTransition.Raise(new RegionConfig()
        {
            Availableregions = regions,
            BestRegion = m_RegionHandler.BestRegion
        });
    }

    public void OnRegionSelect(Region region)
    {
        PhotonNetwork.Disconnect();
        m_IsTestConnection = false;

        string regionCode = region.Code;
        PhotonNetwork.ConnectToRegion(regionCode);

        UpdateConnectionStatus(
            $"Connected To {NetworkManager.Instance.RegionsRegistry.GetRegionName(regionCode)}, Finding Lobby");
        print($"Region : {regionCode} Flow");
    }
    
    public void CreateRoom(RoomOptions roomOptions)
    {
        m_RequiredPlayersCount = roomOptions.MaxPlayers;
        roomOptions.CustomRoomProperties= new ExitGames.Client.Photon.Hashtable() { { "C0", "1" } };
        roomOptions.CustomRoomPropertiesForLobby = new [] { "C0" };
        roomOptions.CleanupCacheOnLeave = false;
        
        PhotonNetwork.JoinOrCreateRoom(Guid.NewGuid().ToString(), roomOptions, TypedLobby.Default);
        UpdateConnectionStatus("Setting Up Room");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        if (!m_IsJoiningRoom)
            return;
        
        OnRoomsReceivedInternal(roomList);
    }

    private void OnRoomsReceivedInternal(List<RoomInfo> roomList)
    {
        Debug.LogError("Rooms Recieved");
        if (!roomList.Any())
        {
            GameEvents.NetworkEvents.RoomJoinFailed.Raise();
            return;
        }
        
        List<string> rooms = new();
        
        foreach (var roomInfo in roomList)
        {
            if (roomInfo.IsOpen)
            {
                rooms.Add(roomInfo.Name);   
            }
        }
        GameEvents.MenuEvents.RoomsListUpdated.Raise(rooms);
        GameEvents.MenuEvents.MenuTransition.Raise(MenuName.RoomSelection);
    }

    public override void OnJoinedLobby()
    {
        GameEvents.NetworkEvents.LobbyJoined.Raise();
        
    }

    public void RequestRandomRoomJoin()
    {
        UpdateConnectionStatus("Joined Lobby, Finding Match");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        UpdateConnectionStatus($"Match Found,Waiting For Others");

        GameEvents.MenuEvents.MenuTransition.Raise(MenuName.InsideRoom);
        GameEvents.NetworkEvents.PlayerJoinedRoom.Raise(PhotonNetwork.IsMasterClient);

        UpdatePlayersList();

        for (int i = 0; i < PhotonNetwork.CurrentRoom.CustomProperties.Keys.ToList().Count; i++)
        {
            Debug.LogError($" Keys {PhotonNetwork.CurrentRoom.CustomProperties.Keys.ToList()[i]}");
        }
        
        UpdateConnectionStatus($"Waiting for host to start the match");

        if (!PhotonNetwork.IsMasterClient)
            return;

        GameEvents.NetworkEvents.GameRoomCreated.Raise();
    }

    private void StartMatchTimer()
    {
        NetworkManager.NetworkUtilities.RaiseRPC(m_PhotonView, nameof(StartMatchTimer_RPC), RpcTarget.All, null);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        GameEvents.NetworkPlayerEvents.OnPlayerDisconnected.Raise();
        UpdatePlayersList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdatePlayersList();
    }

    private void UpdatePlayersList()
    {
        List<string> playerNames = new();
        Player[] players = PhotonNetwork.PlayerList;
        
        for (int i = 0; i < players.Length; i++)
        {
            playerNames.Add(players[i].NickName);
        }
        
        GameEvents.MenuEvents.PlayersListUpdated.Raise(playerNames);
    }

    [PunRPC]
    private void StartMatchTimer_RPC()
    {
        StartMatchInternal();
        //GameEvents.MenuEvents.MenuTransition.Raise(MenuName.ConnectionScreen);
        // GameEvents.TimerEvents.ExecuteActionRequest.Raise(new TimerDataObject()
        // {
        //     JobID = Guid.NewGuid().GetHashCode(),
        //     TimeDuration = 7,
        //     ActionToExecute = StartMatchInternal,
        //     TickEvent = OnStartMatchTick
        // });
    }

    private void OnStartMatchTick(float time)
    {
        m_ControllerView.SetTimerStatus($"Starting The Match in {time}");
    }

    private void StartMatchInternal()
    {
        m_ControllerView.HideTimer();

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.IsOpen = false;
        
        NetworkManager.Instance.LoadGameplay("PokerGame");
    }
}

using System.Collections.Generic;
using Photon.Realtime;

public static partial class GameEvents
{
    public static class NetworkEvents
    {
        public static GameEvent LocalPlayerJoined = new();
        public static GameEvent<PlayerController> NetworkPlayerJoined = new();
        
        public static GameEvent<string, int> PlayerReceiveCardsData = new();
        public static GameEvent PlayersJoined = new();

        public static GameEvent NetworkJoinedEvent = new();
        public static GameEvent NetworkDisconnectedEvent = new();

        public static GameEvent<string, float> NetworkTimerStartRequest = new();

        public static GameEvent OnStartMatch = new();
        public static GameEvent OnRoomJoined = new();
        public static GameEvent OnPlayerRoomActivity = new();
        
        public static GameEvent<Region> OnRegionSelect = new();
        public static GameEvent<RoomOptions> OnRoomSelect = new();
        public static GameEvent<RegionConfig> OnServerConnected = new();
        public static GameEvent OnRoomJoinFailed = new();
    }
}

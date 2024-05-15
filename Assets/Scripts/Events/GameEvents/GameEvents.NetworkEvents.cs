
using System.Collections.Generic;
using Photon.Realtime;

public static partial class GameEvents
{
    public static class NetworkEvents
    {
        public static GameEvent LocalPlayerJoined = new();
        public static GameEvent<PlayerController> NetworkPlayerJoined = new();
        
        public static GameEvent<string, int> PlayerReceiveCardsData = new();

        public static GameEvent NetworkJoinedEvent = new();
        public static GameEvent NetworkDisconnectedEvent = new();

        public static GameEvent<string, float> NetworkTimerStartRequest = new();
        
        public static GameEvent<string> PlayerLogin = new ();
        public static GameEvent<Region> PlayerRegionSelect = new ();
        public static GameEvent<RoomOptions> PlayerRoomCreation = new ();
        
        public static GameEvent RoomJoinFailed = new();
        public static GameEvent<RegionConfig> ConnectionTransition = new();
        public static GameEvent<string> NetworkStatus = new();
        public static GameEvent LobbyJoined = new();
        public static GameEvent PlayerCharacterSelected = new();
        public static GameEvent SelectedCharacter = new();
        public static GameEvent<bool> PlayerJoinedRoom = new();
        public static GameEvent MatchSetupRequested = new();
        
        public static GameEvent GameRoomCreated = new();
    }
}

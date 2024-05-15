
using System.Collections.Generic;
using Photon.Realtime;

public static partial class GameEvents
{
    public static class MenuEvents
    {
        public static GameEvent<string, float> TimeBasedActionRequested = new();
        
        public static GameEvent<MenuName> MenuTransition = new();
        public static GameEvent<List<string>> PlayersListUpdated = new();
        public static GameEvent<List<string>> RoomsListUpdated = new();
        public static GameEvent MatchStartRequested = new();
        public static GameEvent<string> RoomJoinRequested = new();


        
    }
}

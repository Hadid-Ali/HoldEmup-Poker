
using System.Collections.Generic;

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
    }
}

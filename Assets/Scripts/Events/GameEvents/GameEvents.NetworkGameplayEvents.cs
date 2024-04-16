using System.Collections.Generic;

public static partial class GameEvents
{
    public static class NetworkGameplayEvents
    {
        public static readonly GameEvent <NetworkDataObject> NetworkSubmitRequest = new();
        public static readonly GameEvent <List<NetworkDataObject>> AllUserHandsReceived = new();
        public static readonly GameEvent <CardData[], int> UserHandReceivedEvent = new();
        public static readonly GameEvent <List<NetworkDataObject>, List<PlayerScoreObject>> PlayerScoresReceived = new();
        public static readonly GameEvent <PlayerController> PlayerJoinedGame = new();
        public static readonly GameEvent <List<int>,bool> MatchWinnersAnnounced = new();
 
        public static readonly GameEvent ExposePocketCardsLocally = new();
        public static readonly GameEvent <CardData, CardData> OnPocketCardsView = new();
        public static readonly GameEvent <List<int>> OnAllPlayersSeated = new();
        public static readonly GameEvent OnUpdatePlayersView = new();
    }
}

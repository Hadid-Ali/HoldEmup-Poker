using System.Collections.Generic;

public static partial class GameEvents
{
    public static class NetworkGameplayEvents
    {
        public static readonly GameEvent <NetworkDataObject> NetworkSubmitRequest = new();
        public static readonly GameEvent  OnShowDown = new();
        
        public static readonly GameEvent <List<NetworkDataObject>> AllUserHandsReceived = new();
        public static readonly GameEvent <CardData[], int> UserHandReceivedEvent = new();
        public static readonly GameEvent <List<NetworkDataObject>, List<PlayerScoreObject>> PlayerScoresReceived = new();
        public static readonly GameEvent <PlayerController> PlayerJoinedGame = new();
        public static readonly GameEvent <List<int>,bool> MatchWinnersAnnounced = new();
 
        public static readonly GameEvent ExposePocketCardsLocally = new();
        public static readonly GameEvent <CardData, CardData> OnPocketCardsView = new();
        public static readonly GameEvent <CardData[]> OnBoardCardsView = new();
        public static readonly GameEvent  OnBoardCardsViewReset = new();
        public static readonly GameEvent  OnAllPlayersSeated = new();
        public static readonly GameEvent OnUpdatePlayersView = new();
        public static readonly GameEvent OnRoundEnd = new();
        
    }
}

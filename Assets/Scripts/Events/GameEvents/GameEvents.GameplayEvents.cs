using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public static partial class GameEvents
{
    public static class GameplayEvents
    {
        public static GameEvent<CardData, Card> CardDragStartEvent = new();
        public static GameEvent CardDropEvent = new();
        public static GameEvent<CardData> CardReplacedEvent = new();
        
        public static GameEvent<DeckName, HandTypes> CardDeckUpdated = new();
        public static GameEvent<Dictionary<int, PlayerScoreObject>> UserHandsEvaluated = new();
        public static GameEvent<GameplayState> GameplayStateSwitched = new();
        
        public static GameEvent<bool> GameplayCardsStateChanged = new();
        public static GameEvent RoundCompleted = new();
        
        public static GameEvent<PlayerViewDataObject> LocalPlayerJoined = new();
        public static GameEvent<int, int> PlayerScoreReceived = new();

        public static GameEvent<int, Vector3> PlayerPositionInit = new();

        public static GameEvent<int, int> OnPlayerScoreSubmit = new();
    }
}

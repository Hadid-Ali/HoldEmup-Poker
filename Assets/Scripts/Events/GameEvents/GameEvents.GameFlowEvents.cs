using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GameEvents
{
    public static class GameFlowEvents
    {
        public static GameEvent RoundStart = new();
        public static GameEvent RestartRound = new();
        public static GameEvent MatchOver = new();
        public static GameEvent LeaveMatch = new();
    }
}

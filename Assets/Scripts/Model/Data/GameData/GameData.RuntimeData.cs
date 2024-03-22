using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public static partial class GameData 
{
    public static class RuntimeData
    {
        public static bool IS_LOGGED_IN = false;
        public static string USER_NAME = string.Empty;
        
        public static int CURRENT_BOTS_FOR_SPAWNING;
        public static int TOTAL_BOTS_SPAWNED = 0;

        public static int TOTAL_PLAYER_SCORE
        {
            get;
            private set;
        }

        public static void ResetTotalPlayerScore()
        {
            TOTAL_PLAYER_SCORE = 0;
        }

        public static void ResetBotSpawnCount()
        {
            TOTAL_BOTS_SPAWNED = 0;
        }
        
        public static void AddToTotalPlayerScore(int score)
        {
            TOTAL_PLAYER_SCORE += score;
        }
    }
}

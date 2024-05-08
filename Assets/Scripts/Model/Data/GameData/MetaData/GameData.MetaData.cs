using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GameData
{
    public static class MetaData
    {
        public const int HandWinReward = 10;
        public const int DecksCount = 1;
        public const int DeckSize = 5;

        public const int TotalScoreToWin = 100;
        
        public const int OffsetCards = 2;

        public const int MinimumNameLength = 3;
        public const int MaximumNameLength = 10;

        public const int MaxPlayersLimit = 3;
        public const int MinimumRequiredPlayers = 1;
        
        public const int WaitBeforeAutomaticMatchStart = 5;
        public const int WaitBeforePlayerJoinNotify = 6;
        
        public const int NullID = -1000;

        public const string PrivacyPolicyLink = "https://triplehandpoker.com/privacy-policy/";
        public const string TermsOfUsageLink = "https://triplehandpoker.com/terms-and-conditions/";
    }
}
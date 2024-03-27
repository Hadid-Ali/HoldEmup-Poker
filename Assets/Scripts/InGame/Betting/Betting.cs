using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Betting : MonoBehaviour 
{
    [SerializeField] private PlayerSeats playerSeats;
    
     public event Action<NetworkPlayer> PlayerStartBettingEvent;
     public event Action<BetActionInfo> PlayerEndBettingEvent;
     public NetworkPlayer CurrentBetRaiser { get; private set; }
     public int CurrentBetterId => CurrentBetRaiser.id;
     
     public int CallAmount => PlayerSeats.Instance.activePlayers.GroupBy(entry => entry.betAmount)
         .Max(item => item.Count());

     public int BigBlind;
     public int SmallBlind;


     [SerializeField] private float _betTime;

     private int betsCount;
     public bool TurnsCompleted => betsCount >= playerSeats.activePlayers.Count;

     public int BetBlinds(NetworkPlayer smallBlindPlayer)
     {
         SmallBlind = smallBlindPlayer.betAmount;
         BigBlind = SmallBlind * 2;
         
         return 0;
     }

     public void Bet(NetworkPlayer player)
     {
         switch (player.playerAction)
         {
             case PlayerAction.Call:
                 break;
             case PlayerAction.Check:
                 break;
             case PlayerAction.Fold:
                 break;
             case PlayerAction.Raise:
                 break;
         }
         
         betsCount++;
     }
     
     
     
     
     
    
}
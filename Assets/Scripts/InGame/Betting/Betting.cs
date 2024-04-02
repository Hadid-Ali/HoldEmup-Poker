using System;
using System.Collections;
using UnityEngine;

public class Betting : MonoBehaviour 
{
    [SerializeField] private PlayerSeats playerSeats;
    [SerializeField] private Pot pot;
    
     public static event Action<NetworkPlayer> PlayerStartBettingEvent;
     public static event Action<BetActionInfo> PlayerEndBettingEvent;
     public NetworkPlayer CurrentBetRaiser { get; private set; }
     public int CurrentBetterId => CurrentBetRaiser.id;
     
     public int CallAmount;

     public int BigBlind;
     public int SmallBlind;
     
     [SerializeField] private float _betTime;
     private Coroutine turnCoroutine;

     private int betsCount;
     public bool TurnsCompleted => betsCount >= playerSeats.ActivePlayers.Count;

     IEnumerator TurnWaitCoroutine()
     {
         //PlayerStartBettingEvent?.Invoke();
         yield return new WaitForSeconds(_betTime);
         //PlayerEndBettingEvent?.Invoke();
     }

     public void BetBlinds(NetworkPlayer smallBlindPlayer)
     {
         SmallBlind = smallBlindPlayer.betAmount;
         BigBlind = SmallBlind * 2;

         CallAmount = BigBlind;
         
         pot.AddToPot(CallAmount);
     }

     public void Bet(NetworkPlayer player)
     {
         switch (player.lastBetAction)
         {
             case BetAction.Call:
                 break;
             case BetAction.Check:
                 break;
             case BetAction.Fold:
                 break;
             case BetAction.Raise:
                 break;
         }
         
         betsCount++;
     }
     
     
     
     
     
    
}
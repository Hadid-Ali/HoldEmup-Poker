using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Betting : MonoBehaviour 
{
    [SerializeField] private PlayerSeats playerSeats;
    [SerializeField] private TurnSequenceHandler turnSequenceHandler;
    [SerializeField] private Pot pot;
    
     public static Action<NetworkPlayer> PlayerStartBettingEvent;
     public static Action<BetActionInfo> PlayerEndBettingEvent;
     public NetworkPlayer CurrentBetRaiser { get; private set; }

     public int CallAmount;

     public int BigBlind;
     public int SmallBlind;
     
     [SerializeField] private float _betTime;
     private Coroutine turnCoroutine;

     private int betsCount;
     public bool TurnsCompleted => betsCount >= playerSeats.ActivePlayers.Count;

     private void Start()
     {
         PlayerStartBettingEvent += NextTurn;
         PlayerEndBettingEvent += OnBetEnd;
     }
     
     private void OnDestroy()
     {
         PlayerStartBettingEvent -= NextTurn;
         PlayerEndBettingEvent -= OnBetEnd;
     }
     
     IEnumerator TurnWaitCoroutine()
     {
         yield return new WaitForSeconds(_betTime);
         OnBetEnd(new BetActionInfo(CurrentBetRaiser, BetAction.Fold,0));
     }

     public void BetBlinds(NetworkPlayer smallBlindPlayer, NetworkPlayer BigBlindPlayer)
     {
         SmallBlind = smallBlindPlayer.betAmount;
         BigBlind = SmallBlind * 2;

         CallAmount = BigBlind + SmallBlind;
         
         smallBlindPlayer.SubCredit(SmallBlind);
         BigBlindPlayer.SubCredit(BigBlind);
         
         pot.AddToPot(BigBlind + SmallBlind);
     }

     public void EndRound()
     {
         StopCoroutine(turnCoroutine);
         foreach (var v in playerSeats.ActivePlayers)
            v.EnableTurn(false);
         
         turnSequenceHandler.CurrentTurnIndex = 0;
     }
     private void OnBetEnd(BetActionInfo obj)
     {
         if(!PhotonNetwork.IsMasterClient)
             return;
         
         CurrentBetRaiser.EnableTurn(false);
         Bet(CurrentBetRaiser, obj);
         
         NetworkPlayer p =
             playerSeats.ActivePlayers.Find(x => x.id == turnSequenceHandler.TurnSequence[turnSequenceHandler.CurrentTurnIndex]);
         
         StopCoroutine(turnCoroutine);
         
         NextTurn(p);
         
     }

     public void NextTurn(NetworkPlayer p)
     {
         turnSequenceHandler.CurrentTurnIndex++;
         turnCoroutine = StartCoroutine(TurnWaitCoroutine());
         
         CurrentBetRaiser = p;
         
         if(!p.HasFolded)
            p.EnableTurn(true);
         else
             OnBetEnd(new BetActionInfo(CurrentBetRaiser, BetAction.Fold,0));
         
     }

     public void StartTurn(int roundIndex)
     {
         turnSequenceHandler.CurrentTurnIndex = roundIndex switch
         {
             0 => playerSeats.ActivePlayers.Count > 2 ? 2 : 0,
             1 => 0,
             _ => turnSequenceHandler.CurrentTurnIndex
         };

         var p = playerSeats.ActivePlayers.Find(x => x.id == turnSequenceHandler.TurnSequence[turnSequenceHandler.CurrentTurnIndex]);
         
         NextTurn(p);
         print("Working...");
     }
     
     public void Bet(NetworkPlayer player, BetActionInfo obj)
     {
         switch (player.lastBetAction)
         {
             case BetAction.Call:
                 player.SubCredit(CallAmount);
                 pot.AddToPot(CallAmount);
                 break;
             case BetAction.Check:
                 
                 break;
             case BetAction.Fold:
                 player.HasFolded = true;
                 break;
             case BetAction.Raise:
                 CallAmount += obj.BetAmount;
                 player.SubCredit(CallAmount);
                 pot.AddToPot(CallAmount);
                 break;
         }
         
         betsCount++;
     }
     
     
     
     
     
    
}
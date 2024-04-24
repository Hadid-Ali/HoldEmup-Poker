using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class Betting : MonoBehaviour 
{
    [SerializeField] private PlayerSeats playerSeats;
    [SerializeField] private TurnSequenceHandler turnSequenceHandler;
    [SerializeField] private Pot pot;
    
     public static Action<NetworkPlayer> PlayerStartBettingEvent;
     public static Action<BetActionInfo> PlayerEndBettingEvent;
     private NetworkPlayer CurrentBetRaiser { get; set; }

     private int _callAmount;
     private int _lastRaise;

     public int bigBlind;
     public int smallBlind;
     
     [SerializeField] private float betTime;
     private Coroutine _turnCoroutine;

     private int _betsCount;
     public bool TurnsCompleted => _betsCount >= playerSeats.ActivePlayers.Count;

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
         yield return new WaitForSeconds(betTime);
         
         OnBetEnd(new BetActionInfo(CurrentBetRaiser, BetAction.Fold,0));
     }

     public void BetBlinds(NetworkPlayer smallBlindPlayer, NetworkPlayer bigBlindPlayer)
     { 
         smallBlind = smallBlindPlayer.betAmount;
         bigBlind = smallBlind * 2;

         _callAmount = bigBlind + smallBlind;
         
         smallBlindPlayer.SubCredit(smallBlind);
         bigBlindPlayer.SubCredit(bigBlind);
         
         pot.AddToPot(bigBlind + smallBlind);
     }

     public void EndStage()
     {
         StopCoroutine(_turnCoroutine);
         foreach (var v in playerSeats.ActivePlayers)
            v.EnableTurn(false);
         
         turnSequenceHandler.CurrentTurnIndex = 0;
         _betsCount = 0;
         _lastRaise = 0;
     }
     private void OnBetEnd(BetActionInfo obj)
     {
         if(!PhotonNetwork.IsMasterClient)
             return;
         
         CurrentBetRaiser.EnableTurn(false);
         Bet(CurrentBetRaiser, obj);
         
         NetworkPlayer p =
             playerSeats.ActivePlayers.Find(x => x.id == turnSequenceHandler.TurnSequence[turnSequenceHandler.CurrentTurnIndex]);
         
         StopCoroutine(_turnCoroutine);
         
         NextTurn(p);
         
     }

     public void NextTurn(NetworkPlayer p)
     {
         turnSequenceHandler.CurrentTurnIndex++;
         _turnCoroutine = StartCoroutine(TurnWaitCoroutine());
         
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
     }
     
     public void Bet(NetworkPlayer player, BetActionInfo obj)
     {
         switch (player.lastBetAction)
         {
             case BetAction.Call:
                 player.SubCredit(_callAmount);
                 pot.AddToPot(_callAmount);
                 break;
             case BetAction.Check:
                 
                 break;
             case BetAction.Fold:
                 player.HasFolded = true;
                 break;
             case BetAction.Raise:
                 _callAmount += _lastRaise + obj.BetAmount;
                 _lastRaise = obj.BetAmount;
                 
                 player.SubCredit(_callAmount);
                 pot.AddToPot(_callAmount);
                 break;
             case BetAction.AllIn:
                 break;
         }
         
         _betsCount++;
     }
     
     
     
     
     
    
}
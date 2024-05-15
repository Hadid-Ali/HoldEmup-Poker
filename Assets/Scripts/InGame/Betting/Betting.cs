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
     private NetworkPlayer CurrentPlayer { get; set; }
     private NetworkPlayer LastBetRaiser { get; set; }

     [SerializeField] private int _callAmount;
     [SerializeField] private int _lastRaise = 0;

     public int bigBlind;
     public int smallBlind;
     
     [SerializeField] private float betTime;
     private Coroutine _turnCoroutine;

     private int _betsCount;
     private int _raiseCount;
     public bool TurnsCompleted => _betsCount >= playerSeats.activePlayers.Count;
     private bool IsTurnCallEligible => _callAmount > 0;
     private bool IsTurnCheckEligible => _raiseCount <= 0;
     

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
         
         OnBetEnd(new BetActionInfo(CurrentPlayer, BetAction.Fold,0));
     }

     public void BetBlinds(NetworkPlayer smallBlindPlayer, NetworkPlayer bigBlindPlayer)
     { 
         smallBlind = smallBlindPlayer.betAmount;
         bigBlind = smallBlind * 2;

         _callAmount = bigBlind;
         smallBlindPlayer.SetAction(BetAction.SmallBlind);
         bigBlindPlayer.SetAction(BetAction.BigBlind);
         smallBlindPlayer.PlayerCredit.SubCredit(smallBlind);
         bigBlindPlayer.PlayerCredit.SubCredit(bigBlind);
         
         pot.AddToPot(bigBlind + smallBlind);
     }

     public void EndStage()
     {
         StopCoroutine(_turnCoroutine);
         foreach (var v in playerSeats.activePlayers)
         {
            v.EnableTurn(false);
           // v.SetAction(BetAction.UnSelected); 
         }
         
         
         turnSequenceHandler.CurrentTurnIndex = 0;
         LastBetRaiser = null;
         _raiseCount = 0;
         _betsCount = 0;
         _lastRaise = 0;
         _callAmount = 0;
     }
     private void OnBetEnd(BetActionInfo obj)
     {
         if(!PhotonNetwork.IsMasterClient)
             return;

         foreach (var v in playerSeats.activePlayers)
             v.EnableTurn(false);
         
         
         CurrentPlayer.EnableTurn(false);
         Bet(CurrentPlayer, obj);
         
         NetworkPlayer p =  playerSeats.activePlayers.Find(x => x.id == 
                                                      turnSequenceHandler.TurnSequence[turnSequenceHandler.CurrentTurnIndex]);
         
         StopCoroutine(_turnCoroutine);
         
         NextTurn(p);
         
     }

     public void NextTurn(NetworkPlayer p)
     {
         if (p.hasFolded)
         {
             SkipTurn();
             return;
         }
         
         p.EnableAction(BetAction.Call, IsTurnCallEligible);
         p.EnableAction(BetAction.Check, IsTurnCheckEligible);

         bool canAfford = p.PlayerCredit.Credits >= Constants.Player.MaximumRaiseLimit;
         
         int maxAmount = canAfford ? Constants.Player.MaximumRaiseLimit : p.PlayerCredit.Credits;
         int minAmount = canAfford ? _lastRaise : maxAmount; 
         
         turnSequenceHandler.CurrentTurnIndex++;
         
         _turnCoroutine = StartCoroutine(TurnWaitCoroutine());
         
         CurrentPlayer = p;
         
         if (p.hasFolded || (_raiseCount > 0 && LastBetRaiser == p))
         {
             SkipTurn();
             return;
         }
         
         p.OnLocalPlayerRaiseSlideUpdate(minAmount, maxAmount);
         p.EnableTurn(true);
         
     }

     public void SkipTurn()
     {
         _betsCount++;
         NetworkPlayer p =  playerSeats.activePlayers.Find(x => x.id == 
                                                                turnSequenceHandler.TurnSequence[turnSequenceHandler.CurrentTurnIndex]);
         StopCoroutine(_turnCoroutine);
         NextTurn(p);
     }

     public void StartTurn(int roundIndex)
     {
         turnSequenceHandler.CurrentTurnIndex = roundIndex switch
         {
             0 => playerSeats.activePlayers.Count > 2 ? 2 : 0,
             1 => 0,
             _ => turnSequenceHandler.CurrentTurnIndex
         };

         NetworkPlayer p =  playerSeats.activePlayers.Find(x => x.id == 
                                                      turnSequenceHandler.TurnSequence[turnSequenceHandler.CurrentTurnIndex]);
         NextTurn(p);
     }
     
     public void Bet(NetworkPlayer player, BetActionInfo obj)
     {
         bool canAfford = player.PlayerCredit.Credits >= 150;
         switch (player.lastBetAction)
         {
             case BetAction.Call:
                 player.lastBetAction = canAfford? obj.BetAction : BetAction.AllIn;
                 
                 player.PlayerCredit.SubCredit(_callAmount);
                 pot.AddToPot(_callAmount);
                 break;
             case BetAction.Check:
                 
                 break;
             case BetAction.Fold:
                 player.hasFolded = true;
                 break;
             case BetAction.Raise:
                 player.lastBetAction = canAfford? obj.BetAction : BetAction.AllIn;
                 _callAmount = _lastRaise + obj.BetAmount;
                 _lastRaise += obj.BetAmount;
                 
                 player.PlayerCredit.SubCredit(_callAmount);
                 pot.AddToPot(_callAmount);

                 if (_raiseCount <= 0)
                 {
                    _betsCount = 0;
                     LastBetRaiser = player;
                 }

                 _raiseCount++;
                 break;
             case BetAction.AllIn:
                 break;
         }
         
         _betsCount++;
     }
     
     
     
     
     
    
}
using System;
using System.Collections;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class Betting : MonoBehaviour
{
    
    [SerializeField] private PlayerSeats playerSeats;
    [SerializeField] private TurnSequenceHandler turnSequenceHandler;
    [SerializeField] private Pot pot;
    [SerializeField] private Game game;
    
     public static Action<NetworkPlayer> PlayerStartBettingEvent;
     public static Action<BetActionInfo> PlayerEndBettingEvent;
     private NetworkPlayer CurrentPlayer { get; set; }
     private NetworkPlayer LastBetRaiser { get; set; }

     [SerializeField] private int _callAmount;
     [SerializeField] private int _totalRaise = 0;

     private int _bigBlind;
     private int _smallBlind;

     private NetworkPlayer _smallBlindPlayer;
     private NetworkPlayer _bigBlindPlayer;
     
     [SerializeField] private float betTime;
     private Coroutine _turnCoroutine;

     private int _betsCount;
     private int _raiseCount;
     public bool TurnsCompleted => _betsCount >= playerSeats.activePlayers.Count;
     private bool IsPreflop => game.GetCurrentStage() == GameStage.PreFlop;

     private bool IsTurnCallEligible()
     {
         if (IsPreflop)
             return _bigBlindPlayer != CurrentPlayer;
         
         return _callAmount > 0;
     }

     private bool IsTurnCheckEligible()
     {
         if (IsPreflop)
             return _smallBlindPlayer != CurrentPlayer;
         
         print($"Check should work : {_raiseCount <= 0}");
         return _raiseCount <= 0;
     }  
     
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
         _smallBlindPlayer = smallBlindPlayer;
         _bigBlindPlayer = bigBlindPlayer;
         
         _smallBlind = Constants.Player.SmallBlindAmount;
         _bigBlind = _smallBlind * 2;

         _callAmount = _bigBlind;
         _totalRaise = _callAmount;

         smallBlindPlayer.SetAction(BetAction.SmallBlind);
         bigBlindPlayer.SetAction(BetAction.BigBlind);
         smallBlindPlayer.PlayerCredit.SubCredit(_smallBlind);
         bigBlindPlayer.PlayerCredit.SubCredit(_bigBlind);
         
         pot.AddToPot(_bigBlind + _smallBlind);
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
         _smallBlind = Constants.Player.SmallBlindAmount;
         _bigBlind = 0;
         _raiseCount = 0;
         _betsCount = 0;
         _totalRaise = 0;
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
         
         NetworkPlayer p = ValidatePlayerAgainstID();
         
         StopCoroutine(_turnCoroutine);
         
         NextTurn(p);
         
     }

     public void NextTurn(NetworkPlayer p)
     {
         if (p.hasFolded && !TurnsCompleted)

         {
             SkipTurn();
             return;
         }
         CurrentPlayer = p;
         
         p.EnableAction(BetAction.Call, IsTurnCallEligible());
         p.EnableAction(BetAction.Check, IsTurnCheckEligible());

         bool canAfford = p.PlayerCredit.Credits >= Constants.Player.MaximumRaiseLimit;
         
         int maxAmount = canAfford ? Constants.Player.MaximumRaiseLimit : p.PlayerCredit.Credits;
         int minAmount = canAfford ? _callAmount: maxAmount;
         
         if (minAmount < Constants.Player.MinimumRaiseLimit)
             minAmount = Constants.Player.MinimumRaiseLimit;
         
         turnSequenceHandler.CurrentTurnIndex++;
         
         _turnCoroutine = StartCoroutine(TurnWaitCoroutine());
         
         
         if (_raiseCount > 0 && LastBetRaiser == p && !TurnsCompleted)
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
         
         NetworkPlayer p = ValidatePlayerAgainstID();
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

         NetworkPlayer p = ValidatePlayerAgainstID();
         NextTurn(p);
     }

     private NetworkPlayer ValidatePlayerAgainstID()
     {
         NetworkPlayer p =  playerSeats.activePlayers.FirstOrDefault(x => x.id == 
                                                                turnSequenceHandler.TurnSequence[turnSequenceHandler.CurrentTurnIndex]);
         if (p != null) return p;
         
         GameEvents.NetworkPlayerEvents.OnPlayerDisconnected.Raise();
         return null;
     }
     
     public void Bet(NetworkPlayer player, BetActionInfo obj)
     {
         bool canAfford = player.PlayerCredit.Credits >= Constants.Player.MaximumRaiseLimit;
         switch (player.lastBetAction)
         {
             case BetAction.Call:
                 player.lastBetAction = canAfford? obj.BetAction : BetAction.AllIn;
                 int calculatedAmount1 = canAfford ? _callAmount : player.PlayerCredit.Credits;
                 player.PlayerCredit.SubCredit(calculatedAmount1);
                 pot.AddToPot(calculatedAmount1);
                 break;
             case BetAction.Check:
                 
                 break;
             case BetAction.Fold:
                 player.hasFolded = true;
                 break;
             case BetAction.Raise:
                 player.lastBetAction = canAfford? obj.BetAction : BetAction.AllIn;
                 int _raiseAmount = obj.BetAmount;
                 
                 _callAmount = _raiseAmount;
                 _totalRaise += _raiseAmount;

                 int calculatedAmount2 = canAfford ? _totalRaise : player.PlayerCredit.Credits;
                 player.PlayerCredit.SubCredit(calculatedAmount2);
                 pot.AddToPot(calculatedAmount2);

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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Betting betting;
    [SerializeField] private PlayerSeats playerSeats;
    [SerializeField] private Pot pot;
    [SerializeField] private TurnSequenceHandler turnSequenceHandler;
    [SerializeField] private BoardCardsView boardCardsView;
    
    public event Action<GameStage> GameStageBeganEvent;
    public event Action<GameStage> GameStageOverEvent;
    public event Action<WinnerInfo[]> EndDealEvent;
    
    public List<CardData> BoardCards;

    private IEnumerator _stageCoroutine;
    private IEnumerator _startDealWhenСonditionTrueCoroutine;
    private IEnumerator _startDealAfterRoundsInterval;


    private bool StartCondition =>  playerSeats.ActivePlayers.Count >= 2;
    
    [SerializeField] private float _roundsIntervalSeconds;
    [SerializeField] private float _showdownEndTimeSeconds;
    [SerializeField] private float _playerPerformSeatActionTimeoutSeconds;
    
    private GameStage _currentGameStage;

    private void Awake()
    {
        Betting.PlayerEndBettingEvent += OnPlayerBetEnd;
    }

    private void OnDestroy()
    {
        Betting.PlayerEndBettingEvent -= OnPlayerBetEnd;
    }

    private void OnPlayerBetEnd(BetActionInfo obj)
    {
        IEnumerable<NetworkPlayer> validPlayers = playerSeats.ActivePlayers.Where(x => !x.HasFolded);
        var networkPlayers = validPlayers as NetworkPlayer[] ?? validPlayers.ToArray();
        
        Debug.Log($"Valid turns : {networkPlayers.Count()}");
        if (networkPlayers.Count() == 1)
        {
            Debug.Log($"Winner {networkPlayers[0].nickName}");
            StopCoroutine(_stageCoroutine);
            betting.EndRound();
        }
        
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(7);
        StartPoker();
    }

    private void StartPoker()
    {
        if(!PhotonNetwork.IsMasterClient)
            return;
        
        _stageCoroutine = StartPreflop();
        StartCoroutine(_stageCoroutine);
    }


    private IEnumerator StartPreflop()
    {
        NetworkPlayer player1 = playerSeats.ActivePlayers.Find(x=>x.id == turnSequenceHandler.TurnSequence[0]);
        NetworkPlayer player2 = playerSeats.ActivePlayers.Find(x=>x.id == turnSequenceHandler.TurnSequence[1]);

        foreach (var v in playerSeats.ActivePlayers)
            player1.DealCards(v);

        BoardCards = DecksHandler.GetRandomHand(5);
        
        GameEvents.NetworkGameplayEvents.ExposePocketCardsLocally.Raise();
        
        betting.BetBlinds(player1,player2);
    
        betting.StartTurn(0);
        
        yield return new WaitUntil(()=> betting.TurnsCompleted);
        print("Game Turns Completed");
        betting.EndRound();

        CardData[] cards = {BoardCards[0], BoardCards[1], BoardCards[2]};
        
        int[] cardData1 = cards[0].ConvertToIntArray();
        int[] cardData2 = cards[1].ConvertToIntArray();
        int[] cardData3 = cards[2].ConvertToIntArray();
        
        _photonView.RPC(nameof(SyncExposedCards), RpcTarget.All, cardData1,cardData2,cardData3);
        
        yield return new WaitForSeconds(_roundsIntervalSeconds);
    
        StartNextStage(GameStage.Turn);
    }

    private IEnumerator StartTurn()
    {
        betting.StartTurn(1);
        yield return new WaitUntil(()=> betting.TurnsCompleted);
        betting.EndRound();
        
        yield return new WaitForSeconds(_roundsIntervalSeconds);
        
    }
    
    // // Stage like Flop, Turn and River.
    private void StartNextStage(GameStage stage)
    {
        StopCoroutine(_stageCoroutine);
        
        
            
        switch (stage)
        {
            case GameStage.Flop: 
                
                _stageCoroutine = StartTurn();
                StartCoroutine(_stageCoroutine);
                
                break;
            case GameStage.Turn:

                break;
            case GameStage.River:

                break;
            default:
                return;
        }
        
    }
    // private IEnumerator StartMidGameStage()
    // {
    //     if (IsServer == false)
    //     {
    //         Logger.Log("MidGame stage wanted to be performed on client. Aborting...", Logger.LogLevel.Error);
    //         yield break;
    //     }
    //     
    //     if (Betting.IsAllIn == false)
    //     {
    //         int[] turnSequence = _boardButton.GetTurnSequence();
    //         yield return Bet(turnSequence);
    //     }
    //
    //     S_EndStage();
    //
    //     yield return new WaitForSeconds(_roundsIntervalSeconds);
    //     
    //     S_StartNextStage();
    // }
    //
    // private IEnumerator StartShowdown()
    // {
    //     if (IsServer == false)
    //     {
    //         Logger.Log("Showdown stage wanted to performed on client. Aborting...", Logger.LogLevel.Error);
    //         yield break;
    //     }
    //     
    //     int[] turnSequence = _boardButton.GetShowdownTurnSequence();
    //     
    //     List<Player> winners = new();
    //     Hand winnerHand = new();
    //     for (var i = 0; i < turnSequence.Length; i++)
    //     {
    //         Player player = PlayerSeats.Players[turnSequence[i]];
    //         List<CardObject> completeCards = _board.Cards.ToList();
    //         completeCards.Add(player.PocketCard1); completeCards.Add(player.PocketCard2);
    //
    //         Hand bestHand = CombinationСalculator.GetBestHand(new Hand(completeCards));
    //
    //         if (i == 0 || bestHand > winnerHand)
    //         {
    //             winners.Clear();
    //             winners.Add(player);
    //             winnerHand = bestHand;
    //         }
    //         else if (bestHand == winnerHand)
    //         {
    //             winners.Add(player);
    //         }
    //     }
    //     
    //     if (winners.Count == 0)
    //     {
    //         throw new NullReferenceException();
    //     }
    //
    //     S_EndStage();
    //     
    //     yield return new WaitForSeconds(_showdownEndTimeSeconds);
    //
    //     List<WinnerInfo> winnerInfo = new();
    //     foreach (Player winner in winners)
    //     {
    //         winnerInfo.Add(new WinnerInfo(winner.OwnerClientId, Pot.GetWinValue(winner, winners), winnerHand.ToString()));
    //     }
    //
    //     S_EndDeal(winnerInfo.ToArray());
    // }
    //
    // private IEnumerator Bet(int[] turnSequence)
    // {
    //     if (IsServer == false)
    //     {
    //         Logger.Log("Betting wanted to performed on client. Aborting...", Logger.LogLevel.Error);
    //         yield break;
    //     }
    //     
    //     for (var i = 0;; i++)
    //     {
    //         foreach (int index in turnSequence)
    //         {
    //             Player player = PlayerSeats.Players[index];
    //
    //             if (player == null)
    //             {
    //                 continue;
    //             }
    //
    //             yield return Betting.Bet(player);
    //         
    //             List<Player> notFoldPlayers = PlayerSeats.Players.Where(x => x != null && x.BetAction != BetAction.Fold).ToList();
    //             if (notFoldPlayers.Count == 1)
    //             {
    //                 ulong winnerId = notFoldPlayers[0].OwnerClientId;
    //                 WinnerInfo[] winnerInfo = {new(winnerId, Pot.GetWinValue(notFoldPlayers[0], new []{notFoldPlayers[0]}), "opponent(`s) folded")};
    //
    //                 if (_isPlaying.Value == true)
    //                 {
    //                     S_EndDeal(winnerInfo);
    //                 }
    //                 yield break;
    //             }
    //
    //             if (i == 0 || IsBetsEquals() == false)
    //             {
    //                 continue;
    //             }
    //
    //             yield break;
    //         }
    //
    //         if (i != 0 || IsBetsEquals() == false)
    //         {
    //             continue;
    //         }
    //
    //         yield break;
    //     }
    // }

    // private IEnumerator StartDealAfterRoundsInterval()
    // {
    //     yield return new WaitForSeconds(_roundsIntervalSeconds);
    //
    //     PlayerSeats.SitEveryoneWaiting();
    //     PlayerSeats.KickPlayersWithZeroStack();
    //     
    //     if (IsServer == false || _startDealWhenСonditionTrueCoroutine != null)
    //     {
    //         yield break;
    //     }
    //     
    //     _startDealWhenСonditionTrueCoroutine = StartDealWhenСonditionTrue();
    //     yield return StartCoroutine(_startDealWhenСonditionTrueCoroutine);
    //
    //     _startDealAfterRoundsInterval = null;
    // }
    //
    // private IEnumerator StartDealWhenСonditionTrue()
    // {
    //     if (IsServer == false)
    //     {
    //         yield break;
    //     }
    //     
    //     yield return new WaitUntil(() => ConditionToStartDeal == true);
    //
    //     _canPerformSeatAction.Value = true;
    //     
    //     S_StartDeal();
    //
    //     _startDealWhenСonditionTrueCoroutine = null;
    //
    //     yield return new WaitForSeconds(_playerPerformSeatActionTimeoutSeconds);
    //
    //     _canPerformSeatAction.Value = false;
    // }
    //
    // private void SetStageCoroutine(GameStage gameStage)
    // {
    //     switch (gameStage)
    //     {
    //         case GameStage.Preflop:
    //             _stageCoroutine = StartPreflop();
    //             break;
    //         
    //         case GameStage.Flop:
    //         case GameStage.Turn:
    //         case GameStage.River:
    //             _stageCoroutine = StartMidGameStage();
    //             break;
    //         
    //         case GameStage.Showdown:
    //             _stageCoroutine = StartShowdown();
    //             break;
    //         
    //         default:
    //             throw new ArgumentOutOfRangeException(nameof(_currentGameStage), _currentGameStage.Value, null);
    //     }
    // }
    //
    // private static bool IsBetsEquals()
    // {
    //     return PlayerSeats.Players.Where(x => x != null && x.BetAction != BetAction.Fold).Select(x => x.BetAmount).Distinct().Skip(1).Any() == false;
    // }
    //
    // private void SetPlayersPocketCards()
    // {
    //     if (IsServer == false)
    //     {
    //         return;
    //     }
    //     
    //     int[] turnSequence = _boardButton.GetTurnSequence();
    //     foreach (int index in turnSequence)
    //     {
    //         CardObject card1 = _cardDeck.PullCard();
    //         CardObject card2 = _cardDeck.PullCard();
    //         
    //         Player player = PlayerSeats.Players[index];
    //         if (player == null)
    //         {
    //             return;
    //         }
    //     
    //         player.SetLocalPocketCards(card1, card2, player.OwnerClientId);
    //
    //         Logger.Log($"Player ('{player}') received: {card1}, {card2}.");
    //     }
    // }
    //
    // private void InitBoard()
    // {
    //     _board = new Board(_cardDeck.PullCards(5).ToList());
    //
    //     if (IsServer == true)
    //     {
    //         Logger.Log($"Board created: {string.Join(", ", _board.Cards)}.");
    //     }
    // }
    //
    // #region Server
    //
    // private void S_StartDeal()
    // {
    //     if (IsServer == false)
    //     {
    //         return;
    //     }
    //     
    //     Logger.Log("Starting Deal.");
    //
    //     _cardDeck = new CardDeck();
    //     
    //     Logger.Log($"Deck created: {string.Join(", ", _cardDeck)}.");
    //     
    //     SetPlayersPocketCards();
    //     InitBoard();
    //
    //     _boardButton.Move();
    //
    //     _isPlaying.Value = true;
    //
    //     S_StartNextStage();
    // }
    //
    // private void S_EndDeal(WinnerInfo[] winnerInfo)
    // {
    //     if (IsServer == false)
    //     {
    //         return;
    //     }
    //
    //     if (_isPlaying.Value == false)
    //     {
    //         Logger.Log($"Trying to EndDeal when it`s already Ended.", Logger.LogLevel.Error);
    //         return;
    //     }
    //
    //     _currentGameStage.Value = GameStage.Empty;
    //     _isPlaying.Value = false;
    //     _board.Cards.Clear();
    //     
    //     List<Player> winners = PlayerSeats.Players.Where(x => x != null && winnerInfo.Select(info => info.WinnerId).Contains(x.OwnerClientId)).ToList();
    //
    //     Logger.Log($"End deal. Pot {winnerInfo[0].Chips}. Winner(`s): ({string.Join(", ", winners)}). Winner hand: {winnerInfo[0].Combination}");
    //
    //     if (_stageCoroutine != null)
    //     {
    //         StopCoroutine(_stageCoroutine);
    //     }
    //     
    //     if (IsHost == false)
    //     {
    //         InitEndDealRoutine(winnerInfo);
    //     }
    //     
    //     EndDealClientRpc(winnerInfo);
    // }
    //

    //
    // private void S_EndStage()
    // {
    //     if (IsServer == false)
    //     {
    //         return;
    //     }
    //
    //     if (IsHost == false)
    //     {
    //         InvokeGameStageOverEvent(_currentGameStage.Value);
    //     }
    //     
    //     EndStageClientRpc(_currentGameStage.Value);
    // }
    //
    // #endregion
    //
    // #region RPC
    [PunRPC]
    private void SyncExposedCards(int[] card1,int[] card2,int[] card3)
    {
        CardData cardd1 = CardData.ConvertBinaryToCardData(card1);
        CardData cardd2 = CardData.ConvertBinaryToCardData(card2);
        CardData cardd3 = CardData.ConvertBinaryToCardData(card3);

        CardData[] cardDatas = { cardd1, cardd2, cardd3 };
        boardCardsView.ExposeCards(cardDatas);                         
    }
    
    //
    // [ClientRpc]
    // private void EndDealClientRpc(WinnerInfo[] winnerInfo)
    // {
    //     _board.Cards.Clear();
    //     InitEndDealRoutine(winnerInfo);
    // }
    //
    // [ClientRpc]
    // private void SetBoardCardsClientRpc(int[] codedBoardCardsString)
    // {
    //     if (IsHost == true)
    //     {
    //         return;
    //     }
    //     
    //     IEnumerable<CardObject> cardObjects = CardObjectConverter.GetCards(codedBoardCardsString);
    //     _board.AddCards(cardObjects);
    // }
    //
    // [ClientRpc]
    // private void StartNextStageClientRpc(GameStage stage)
    // {
    //     InvokeGameStageBeganEvent(stage);
    // }
    //
    // [ClientRpc]
    // private void EndStageClientRpc(GameStage stage)
    // {
    //     InvokeGameStageOverEvent(stage);
    // }
    //
    // #endregion
    //
    // #region Methods that has to be called both on Server and Client.
    //
    // private void InitEndDealRoutine(WinnerInfo[] winnerInfo)
    // {
    //     if (_startDealAfterRoundsInterval != null)
    //     {
    //         StopCoroutine(_startDealAfterRoundsInterval);
    //     }
    //
    //     _startDealAfterRoundsInterval = StartDealAfterRoundsInterval();
    //     StartCoroutine(_startDealAfterRoundsInterval);
    //     
    //     EndDealEvent?.Invoke(winnerInfo);
    // }
    //
    // private void InvokeGameStageBeganEvent(GameStage stage)
    // {
    //     GameStageBeganEvent?.Invoke(stage);
    // }
    //
    // private void InvokeGameStageOverEvent(GameStage stage)
    // {
    //     GameStageOverEvent?.Invoke(stage);
    // }
    //
    // #endregion
}
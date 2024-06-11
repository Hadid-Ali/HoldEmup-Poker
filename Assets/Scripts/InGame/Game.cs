using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Betting betting;
    [SerializeField] private PlayerSeats playerSeats;
    [SerializeField] private Pot pot;
    [SerializeField] private TurnSequenceHandler turnSequenceHandler;
    [SerializeField] private BoardCards boardCards;
    [SerializeField] private DialogBox box;
    
    public event Action<GameStage> GameStageBeganEvent;
    public event Action<GameStage> GameStageOverEvent;
    public event Action<WinnerInfo[]> EndDealEvent;
    


    private IEnumerator _stageCoroutine;
    private IEnumerator _startDealWhenСonditionTrueCoroutine;
    private IEnumerator _startDealAfterRoundsInterval;


    private bool StartCondition =>  playerSeats.activePlayers.Count >= 2;
    
    [SerializeField] private float _roundsIntervalSeconds;
    
    private int _currentGameStageInt;

    public GameStage GetCurrentStage() => (GameStage)_currentGameStageInt;

    private int _boardCardExposeLength = 3;
    private int _continueConsentCollected = 0;
    private int _unFoldedPlayersCount;
    private List<NetworkDataObject> playerCards = new();

    private bool RoundRestartCondition => _continueConsentCollected >= 1;

    private void Awake()
    {
        Betting.PlayerEndBettingEvent += OnPlayerBetEnd;
        GameEvents.NetworkGameplayEvents.NetworkSubmitRequest.Register(OnPlayerCardsReceived);
        GameEvents.NetworkGameplayEvents.OnRoundEnd.Register(ResetGame);
        GameEvents.GameplayEvents.UserHandsEvaluated.Register(OnHandsEvaluated);
        
        GameEvents.NetworkGameplayEvents.OnContinueConsentCollected.Register(ContinueConsentCollected);
       
    }
    private void OnDestroy()
    {
        Betting.PlayerEndBettingEvent -= OnPlayerBetEnd;
        GameEvents.NetworkGameplayEvents.NetworkSubmitRequest.UnRegister(OnPlayerCardsReceived);
        GameEvents.NetworkGameplayEvents.OnRoundEnd.UnRegister(ResetGame);
        GameEvents.GameplayEvents.UserHandsEvaluated.UnRegister(OnHandsEvaluated);
        GameEvents.NetworkGameplayEvents.OnContinueConsentCollected.UnRegister(ContinueConsentCollected);
    }
    
    private void OnPlayerCardsReceived(NetworkDataObject obj)
    {
        if(!PhotonNetwork.IsMasterClient)
            return;
        
        playerCards.Add(obj);
        
        if(playerCards.Count < _unFoldedPlayersCount)
            return;

        StartCoroutine(LazyCalculation());
    }

    IEnumerator LazyCalculation()
    {
        foreach (var v in playerCards)
        {
            v.PlayerDecks.AddRange(boardCards.GetCards);
            
            Handd hand = new Handd();
            foreach (var j in v.PlayerDecks)
                hand.Add(j);
            
            hand = CombinationСalculator.GetBestHanddEfficiently(hand);
            
            v.PlayerDecks.Clear();
            v.PlayerDecks = hand._Handd;
            
            yield return new WaitForSeconds(.3f);
        }
        
        GameEvents.NetworkGameplayEvents.AllUserHandsReceived.Raise(playerCards);
    }

    private void OnPlayerBetEnd(BetActionInfo obj)
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        
        if (!CheckIfLonePlayer()) yield break;
        
        StopCoroutine(_stageCoroutine);
        betting.EndStage();
        StartNextStage(GameStage.Showdown);

    }
    private void OnHandsEvaluated(Dictionary<int, PlayerScoreObject> obj)
    {
        foreach (var v in obj)
        {
            print($"{playerSeats.activePlayers.Find(x=>x.id == v.Key).nickName} : {v.Value.Score}" );
        }
        PlayerScoreObject pp = obj.First(x => x.Value.Score >= 10).Value;

        NetworkPlayer p = playerSeats.activePlayers.Find(x => pp.UserID == x.id);
        p.PlayerCredit.AddCredit(pot.GetPotMoney);

        photonView.RPC(nameof(OnPlayerWin), RpcTarget.All, p.id, pot.GetPotMoney);
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
        
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.Raise();
        StartNextStage(GameStage.PreFlop);
    }


    private IEnumerator StartPreflop()
    {
        _currentGameStageInt = (int)GameStage.PreFlop;
        
        NetworkPlayer player1 = playerSeats.activePlayers.Find(x=>x.id == turnSequenceHandler.TurnSequence[0]);
        NetworkPlayer player2 = playerSeats.activePlayers.Find(x=>x.id == turnSequenceHandler.TurnSequence[1]);

        foreach (var v in playerSeats.activePlayers)
        {
            v.DealCards();
        }


        betting.BetBlinds(player1,player2);

        int bokenPlayers = playerSeats.activePlayers.Count;
        foreach (var v in playerSeats.activePlayers)
        {
            v.hasFolded = v.PlayerCredit.IsBroke; //Disable the broke player
            if (v.PlayerCredit.IsBroke)
                bokenPlayers--;
        }
        if(bokenPlayers <= 1)
            photonView.RPC(nameof(EndGameForAll), RpcTarget.All);


        betting.StartTurn(0);
        
        yield return new WaitUntil(()=> betting.TurnsCompleted);
        betting.EndStage();
        
        boardCards.PopulateCards();
        boardCards.ExposeCards(_boardCardExposeLength);
        _boardCardExposeLength++;
        
        yield return new WaitForSeconds(_roundsIntervalSeconds);
        _currentGameStageInt = (int)GameStage.Flop;
        StartNextStage((GameStage) _currentGameStageInt);
    }

    private IEnumerator MidStage()
    {
        betting.StartTurn(1);
        yield return new WaitUntil(()=> betting.TurnsCompleted);
        betting.EndStage();
        
        boardCards.ExposeCards(_boardCardExposeLength);
        
        if(_boardCardExposeLength < 5)
            _boardCardExposeLength++;

        CheckIfLonePlayer();
        
        yield return new WaitForSeconds(_roundsIntervalSeconds);
        
        _currentGameStageInt++;
        print( $"Next Stage is : {(GameStage) _currentGameStageInt}");
        StartNextStage((GameStage) _currentGameStageInt);
        
    }

    private bool CheckIfLonePlayer()
    {
        IEnumerable<NetworkPlayer> validPlayers = playerSeats.activePlayers.Where(x => !x.hasFolded);
        var networkPlayers = validPlayers as NetworkPlayer[] ?? validPlayers.ToArray();

        return networkPlayers.Count() == 1;
    }
    
    // // Stage like Flop, Turn and River.
    private void StartNextStage(GameStage stage)
    {
        
        switch (stage)
        {
            case GameStage.PreFlop:
                _stageCoroutine = StartPreflop();
                StartCoroutine(_stageCoroutine);
                break;
            case GameStage.Flop:
                StopCoroutine(_stageCoroutine);
                _stageCoroutine = MidStage();
                StartCoroutine(_stageCoroutine);
                break;
            case GameStage.Turn:
                StopCoroutine(_stageCoroutine);
                _stageCoroutine = MidStage();
                StartCoroutine(_stageCoroutine);
                break;
            case GameStage.River:
                StopCoroutine(_stageCoroutine);
                _stageCoroutine = MidStage();
                StartCoroutine(_stageCoroutine);
                break;
            case GameStage.Showdown:
                StopCoroutine(_stageCoroutine);
                _stageCoroutine = StartShowdown();
                StartCoroutine(_stageCoroutine);
                
                break;
            default:
                return;
        }
        
    }

    private void ContinueConsentCollected()
    {
        _continueConsentCollected++;
        print($"Consent Collected {_continueConsentCollected}");
    } 

    private IEnumerator StartShowdown()
    {
        _unFoldedPlayersCount = playerSeats.activePlayers.Count(x => !x.hasFolded);

        if (_unFoldedPlayersCount == 1)
        {
            var p = playerSeats.activePlayers.Find(x => !x.hasFolded);
            p = playerSeats.activePlayers.Find(x => p.id == x.id);
            p.PlayerCredit.AddCredit(pot.GetPotMoney);
            
            //GameEvents.NetworkGameplayEvents.OnShowDown.Raise();
            photonView.RPC(nameof(OnPlayerWin), RpcTarget.All, p.id, pot.GetPotMoney);
        }
        else
        {
            GameEvents.NetworkGameplayEvents.OnShowDown.Raise();
        }

        yield return new WaitUntil(()=> RoundRestartCondition);
        photonView.RPC(nameof(ResetGameView), RpcTarget.All);
    }

    private void ResetGame()
    {
        _boardCardExposeLength = 3;
        _continueConsentCollected = 0;
        _currentGameStageInt = (int) GameStage.Flop;
        
        playerCards.Clear();

        foreach (var v in playerSeats.activePlayers)
            v.hasFolded = false;
        
        StartCoroutine(Start());
    }

    [PunRPC]
    private void EndGameForAll()
    {
        box.Initialize("Match can't start, Players are broke", PhotonNetwork.Disconnect);
    }

    [PunRPC]
    private void OnPlayerWin(int playerId, int winAmount)
    {
        GameEvents.NetworkGameplayEvents.OnPlayerWin.Raise(playerId, winAmount);
    }
    [PunRPC]
    private void ResetGameView()
    {
        GameEvents.NetworkGameplayEvents.OnRoundEnd.Raise();
    }
}
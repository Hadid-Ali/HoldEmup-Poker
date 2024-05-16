using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayersViewHandler : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private List<PlayerView> playerViews;
    [SerializeField] private PlayerSeats playerSeats;
    [SerializeField] private TurnSequenceHandler sequenceHandler;

    private int _lastTurnPlayer = -1;

    private void Awake()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.Register(InitializePlayerView);
        
        GameEvents.NetworkPlayerEvents.OnPlayerTurn.Register(OnPlayerTurn);
        GameEvents.NetworkPlayerEvents.OnPlayerCreditsChanged.Register(OnPlayerCreditsChanged);
        GameEvents.NetworkPlayerEvents.OnPlayerActionPop.Register(OnPlayerAction);
        
        GameEvents.NetworkGameplayEvents.OnShowDown.Register(ExposeAllPocketCards);
        GameEvents.NetworkPlayerEvents.ExposePocketCardsLocally.Register(ExposeLocalCards);
        GameEvents.NetworkGameplayEvents.OnRoundEnd.Register(ResetView);
        GameEvents.NetworkGameplayEvents.OnPlayerWin.Register(OnPlayerWin);
        
    }

    private void ExposeLocalCards(List<CardData> obj)
    {
        playerViews[0].UpdateCardsView(obj[0],obj[1]);
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.UnRegister(InitializePlayerViews_RPC);
        
        GameEvents.NetworkPlayerEvents.OnPlayerTurn.UnRegister(OnPlayerTurn);
        GameEvents.NetworkPlayerEvents.OnPlayerCreditsChanged.UnRegister(OnPlayerCreditsChanged);
        GameEvents.NetworkPlayerEvents.OnPlayerActionPop.UnRegister(OnPlayerAction);
        
        GameEvents.NetworkGameplayEvents.OnShowDown.UnRegister(ExposeAllPocketCards);
        GameEvents.NetworkPlayerEvents.ExposePocketCardsLocally.UnRegister(ExposeLocalCards);
        GameEvents.NetworkGameplayEvents.OnRoundEnd.UnRegister(ResetView);
        GameEvents.NetworkGameplayEvents.OnPlayerWin.UnRegister(OnPlayerWin);
    }

    private void OnPlayerTurn(int id,bool arg2) => photonView.RPC(nameof(SyncPlayerTurn), RpcTarget.All,id,arg2);
    private void OnPlayerCreditsChanged(int arg1, int arg2) => photonView.RPC(nameof(SyncPlayerCredits), RpcTarget.All,arg1,arg2);
    private void OnPlayerAction(int arg1, string arg2) => photonView.RPC(nameof(SyncPlayerAction), RpcTarget.All,arg1,arg2);


    private void InitializePlayerView()
    {
        photonView.RPC(nameof(InitializePlayerViews_RPC), RpcTarget.All);
    }

    private void OnPlayerWin(int pID, int amount) => photonView.RPC(nameof(UpdatePlayerWin), RpcTarget.All, pID, amount);
    private PlayerView GetPlayerViewAgainstID(int id) => playerViews.Find(x => x.playerID == id);

    [PunRPC]
    private void UpdatePlayerWin(int playerID, int amount)
    {
        PlayerView p = GetPlayerViewAgainstID(playerID);
        
        if(p == null)
            return;
        
        p.UpdateWinnerView(true,amount); 
    }

    [PunRPC]
    private void ResetView()
    {
        for (int i = 0; i < playerViews.Count; i++)
        {
            if (i >= playerSeats.activePlayers.Count)
                break;
            
            NetworkPlayer p = playerSeats.activePlayers.Find(x => x.id == sequenceHandler.TurnViewSequence[i]);
            
            if (p == null)
                continue;

            CardData card = new CardData
            {
                value = CardValue.valueS_NO,
                type = CardType.TYPES_NO
            };
            
            playerViews[i].UpdateCardsView(card, card);
            playerViews[i].UpdateWinnerView(false,0);
            playerViews[i].animationSlide.Awake();
        }
    }
    

    [PunRPC]
    private void ExposeAllPocketCards()
    {
        foreach (var t in playerViews)
        {
            if(!t.IsOccupied)
                return;

            PlayerView view = GetPlayerViewAgainstID(t.playerID);
          //  NetworkPlayer p = Dependencies.PlayersContainer.GetPlayerAgainstID(t.playerID);
           // view.UpdateCardsView();
        }
    }

    [PunRPC]
    private void SyncPlayerAction(int id, string actionText)
    {
        PlayerView view = GetPlayerViewAgainstID(id);
        view.PopDialog(actionText, 2);
    }

    [PunRPC]
    private void SyncPlayerCredits(int id, int credits)
    {
        PlayerView view = GetPlayerViewAgainstID(id);
        view.UpdateCreditView(credits);
    }

    [PunRPC]
    private void SyncPlayerTurn(int id, bool isOn)
    {
        PlayerView view = GetPlayerViewAgainstID(id);
        view.UpdateTurnView(isOn);
    }
    
    [PunRPC]
    private void InitializePlayerViews_RPC()
    {
        if(playerSeats.activePlayers.Count <= 0)
            return;
        
        for (int i = 0; i < playerViews.Count; i++)
        {
            if (i >= playerSeats.activePlayers.Count)
                continue;
            
            var p = playerSeats.activePlayers.FirstOrDefault(x => x.id == sequenceHandler.TurnViewSequence[i]);
            
            if (p == null)
                continue;
            
            playerViews[i].Initialize(p.nickName,p.id,p.PlayerCredit.Credits);
        }
    }

    
}



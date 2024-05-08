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

    private void Awake()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.Register(SyncPlayerView);
        GameEvents.NetworkGameplayEvents.OnUpdatePlayersView.Register(SyncPlayerView);
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.Register(UpdateLocalCardsView);
        
        GameEvents.NetworkGameplayEvents.OnShowDown.Register(ExposeAllPocketCards);
        GameEvents.NetworkGameplayEvents.OnRoundEnd.Register(ResetView);
        GameEvents.NetworkGameplayEvents.OnPlayerWin.Register(OnPlayerWin);
        
    }

    private void UpdateLocalCardsView(CardData arg1, CardData arg2)
    {
        playerViews[0].UpdateCardView(arg1, arg2);
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.UnRegister(SyncPlayerView);
        GameEvents.NetworkGameplayEvents.OnUpdatePlayersView.UnRegister(SyncPlayerView);
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.UnRegister(UpdateLocalCardsView);
        
        GameEvents.NetworkGameplayEvents.OnShowDown.UnRegister(ExposeAllPocketCards);
        GameEvents.NetworkGameplayEvents.OnRoundEnd.UnRegister(ResetView);
        GameEvents.NetworkGameplayEvents.OnPlayerWin.UnRegister(OnPlayerWin);
    }

    private void SyncPlayerView()
    {
        if(playerSeats.activePlayers.Count >= 0)
            photonView.RPC(nameof(ArrangePlayersView), RpcTarget.All);
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
            
            playerViews[i].UpdateCardView(card, card);
            playerViews[i].UpdateWinnerView(false,0);
        }
    }

    [PunRPC]
    private void ExposeAllPocketCards()
    {
        for (int i = 0; i < playerViews.Count; i++)
        {
            if (i >= playerSeats.activePlayers.Count)
                break;
            
            NetworkPlayer p = playerSeats.activePlayers.Find(x => x.id == sequenceHandler.TurnViewSequence[i]);
            
            if (p == null)
                continue;
            
            playerViews[i].UpdateCardView(p.pocketCard1,p.pocketCard2);
        }
    }
    
    [PunRPC]
    private void ArrangePlayersView()
    {
        print($"Count : {playerSeats.activePlayers.Count}");
        if(playerSeats.activePlayers.Count <= 0)
            return;
        
        for (int i = 0; i < playerViews.Count; i++)
        {
            if (i >= playerSeats.activePlayers.Count)
            {
                playerViews[i].UpdateView(0);
                continue;
            }
            
            var p = playerSeats.activePlayers.FirstOrDefault(x => x.id == sequenceHandler.TurnViewSequence[i]);
            
            if (p == null)
                continue;
            
            playerViews[i].playerID = p.id;
            playerViews[i].playerName = p.HasFolded? "Has Folded" : p.nickName;
            playerViews[i].playerCredit = p.totalCredit;
            playerViews[i].isOnTurn = p.isOnTurn;
            playerViews[i].lastAction = p.lastBetAction;
            
            playerViews[i].UpdateView();
        }
    }

    
}



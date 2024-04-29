using Photon.Pun;
using UnityEngine;

public class PlayersViewHandler : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private PlayerView[] playerViews;
    [SerializeField] private PlayerSeats playerSeats;
    [SerializeField] private TurnSequenceHandler sequenceHandler;

    private void Awake()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.Register(SyncPlayerView);
        GameEvents.NetworkGameplayEvents.OnUpdatePlayersView.Register(SyncPlayerView);
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.Register(UpdateLocalCardsView);
        
        GameEvents.NetworkGameplayEvents.OnShowDown.Register(ExposeAllPocketCards);
        GameEvents.NetworkGameplayEvents.OnRoundEnd.Register(ResetView);
        
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
    }

    private void SyncPlayerView()
    {
        if(playerSeats.activePlayers.Count >= 0)
            photonView.RPC(nameof(ArrangePlayersView), RpcTarget.All);
    }

    [PunRPC]
    private void ResetView()
    {
        for (int i = 0; i < playerViews.Length; i++)
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
        }
    }

    [PunRPC]
    private void ExposeAllPocketCards()
    {
        for (int i = 0; i < playerViews.Length; i++)
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
        for (int i = 0; i < playerViews.Length; i++)
        {
            if (i >= playerSeats.activePlayers.Count)
            {
                playerViews[i].UpdateView(0);
                continue;
            }

            NetworkPlayer p = playerSeats.activePlayers.Find(x => x.id == sequenceHandler.TurnViewSequence[i]);
            
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

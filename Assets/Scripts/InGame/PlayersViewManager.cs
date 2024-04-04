using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class PlayersViewManager : MonoBehaviour
{
    [SerializeField] private PlayerView[] playerViews;
    [SerializeField] private PlayerSeats playerSeats;
    [SerializeField] private TurnSequenceHandler sequenceHandler;

    private void Awake()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.Register(ArrangePlayersView);
        GameEvents.NetworkGameplayEvents.OnUpdatePlayersView.Register(()=>ArrangePlayersView(sequenceHandler.TurnViewSequence));
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.Register(UpdateLocalCardsView);
        
        
    }

    private void UpdateLocalCardsView(CardData arg1, CardData arg2)
    {
        playerViews[0].UpdateCardView(arg1, arg2);
        print("Cards Updated");
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.UnRegister(ArrangePlayersView);
        GameEvents.NetworkGameplayEvents.OnUpdatePlayersView.UnRegister(()=>ArrangePlayersView(sequenceHandler.TurnViewSequence));
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.UnRegister(UpdateLocalCardsView);
        
    }
    

    private void ArrangePlayersView(List<int> turnSequence)
    {
        for (int i = 0; i < playerViews.Length; i++)
        {
            if (i >= playerSeats.ActivePlayers.Count)
            {
                playerViews[i].playerID = 0;
                playerViews[i].playerName = "Not Joined";
                playerViews[i].playerCredit = 0;
                
                playerViews[i].UpdateView();
                continue;
            }
            
            NetworkPlayer p = playerSeats.ActivePlayers.Find(x => x.id == turnSequence[i]);
            
            if (p == null)
                continue;
            
            playerViews[i].playerID = p.id;
            playerViews[i].playerName = p.HasFolded? "Has Folded" : p.nickName;
            playerViews[i].playerCredit = p.totalCredit;
            playerViews[i].isOnTurn = p.isOnTurn;
            
            playerViews[i].UpdateView();
        }
    }

    
}

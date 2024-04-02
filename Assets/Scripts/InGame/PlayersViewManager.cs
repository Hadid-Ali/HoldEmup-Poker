using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class PlayersViewManager : MonoBehaviour
{
    [SerializeField] private PlayerView[] playerViews;
    [SerializeField] private PlayerSeats playerSeats;

    private void Awake()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.Register(ArrangePlayersView);
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
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.UnRegister(UpdateLocalCardsView);
        
    }

    private void ArrangePlayersView(List<int> turnSequence)
    {
        for (int i = 0; i < turnSequence.Count; i++)
        {
            if(!playerSeats.ActivePlayers.ContainsKey(turnSequence[i]))
                continue;
            
            NetworkPlayer p = playerSeats.ActivePlayers[turnSequence[i]];
            playerViews[i].playerID = p.PhotonPlayer.ActorNumber;
            playerViews[i].playerName = p.PhotonPlayer.NickName;
            
            playerViews[i].UpdateView();
        }
    }

    
}

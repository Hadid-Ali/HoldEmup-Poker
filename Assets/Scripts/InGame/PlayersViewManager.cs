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
        playerViews[1].UpdateCardView(arg1, arg2);
        print("Cards Updated");
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.UnRegister(ArrangePlayersView);
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.UnRegister(UpdateLocalCardsView);
        
    }

    private void ArrangePlayersView(int[] turnSequence)
    {
        for (int i = 1; i < playerViews.Length; i++)
        {
            if(!playerSeats.ActivePlayers.ContainsKey(turnSequence[i-1]))
                continue;

            NetworkPlayer p = playerSeats.ActivePlayers[turnSequence[i-1]];
            playerViews[i].playerID = p.id;
            playerViews[i].playerName = p.nickName;
            
            playerViews[i].UpdateView();
        }
    }

    
}

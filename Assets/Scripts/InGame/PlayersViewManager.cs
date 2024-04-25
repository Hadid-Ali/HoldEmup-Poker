using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class PlayersViewManager : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private PlayerView[] playerViews;
    [SerializeField] private PlayerSeats playerSeats;
    [SerializeField] private TurnSequenceHandler sequenceHandler;

    private void Awake()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.Register(SyncPlayerView);
        GameEvents.NetworkGameplayEvents.OnUpdatePlayersView.Register(()=>SyncPlayerView(sequenceHandler.TurnViewSequence));
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.Register(UpdateLocalCardsView);
        
        
    }

    private void UpdateLocalCardsView(CardData arg1, CardData arg2)
    {
        playerViews[0].UpdateCardView(arg1, arg2);
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.UnRegister(SyncPlayerView);
        GameEvents.NetworkGameplayEvents.OnUpdatePlayersView.UnRegister(()=>SyncPlayerView(sequenceHandler.TurnViewSequence));
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.UnRegister(UpdateLocalCardsView);
        
    }

    private void SyncPlayerView(List<int> turnSequence)
    {
        int[] turnSeq = turnSequence.ToArray();
        
        object[] arr = new object[turnSeq.Length];
        
        for (int i = 0; i < turnSeq.Length; i++)
            arr[i] = turnSeq[i];
        
        
        photonView.RPC(nameof(ArrangePlayersView), RpcTarget.All);
    }
    
    [PunRPC]
    private void ArrangePlayersView()
    {
        for (int i = 0; i < playerViews.Length; i++)
        {
            if (i >= playerSeats.ActivePlayers.Count)
            {
                playerViews[i].UpdateView(0);
                continue;
            }

            NetworkPlayer p = playerSeats.ActivePlayers.Find(x => x.id == sequenceHandler.TurnViewSequence[i]);
            
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

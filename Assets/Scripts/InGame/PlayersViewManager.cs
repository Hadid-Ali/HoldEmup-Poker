using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayersViewManager : MonoBehaviour
{
    [SerializeField] private PlayerView[] playerViews;
    [SerializeField] private PlayerSeats playerSeats;

    private int localPlayerId = PhotonNetwork.LocalPlayer.ActorNumber;

    private void Awake()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.Register(ArrangePlayersView);
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.Register(UpdateCardView);
    }

    private void UpdateCardView(CardData arg1, CardData arg2)
    {
        playerViews[0].UpdateCardView(arg1, arg2);
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.UnRegister(ArrangePlayersView);
        GameEvents.NetworkGameplayEvents.OnPocketCardsView.UnRegister(UpdateCardView);
        
    }

    private void ArrangePlayersView(int[] turnSequence)
    {
        print("turn Id l :" + turnSequence.Length);
        int[] sortedIds = new int[turnSequence.Length]; 
           sortedIds =  RotateOrder(turnSequence);

        print("Sorted Id l :" + sortedIds.Length);
        for (int i = 0; i < playerViews.Length; i++)
        {
            playerViews[i].playerID = sortedIds[i];
            
           //  name = playerSeats.activePlayers.Any(x => x.id == sortedIds[i]);
          //  playerViews[i].playerName = name;
            
            playerViews[i].UpdateView(sortedIds[i], "Player");
        }
    }

    int[] RotateOrder(int[] turnSequence)
    {
        int localPlayerIndex = Array.IndexOf(turnSequence, localPlayerId);
        int[] rotatedPlayerIds = new int[turnSequence.Length];
        
        for (int i = 0; i < turnSequence.Length; i++)
        {
            int turnIndex = (localPlayerIndex + i) % turnSequence.Length;
            int playerId = turnSequence[turnIndex];
            rotatedPlayerIds[i] = playerId;
        }
        
        int localPlayerOrderIndex = Array.IndexOf(rotatedPlayerIds, localPlayerId);
        
        Array.Copy(rotatedPlayerIds, localPlayerOrderIndex, rotatedPlayerIds, 0, turnSequence.Length - localPlayerOrderIndex);
        Array.Copy(rotatedPlayerIds, 0, rotatedPlayerIds, turnSequence.Length - localPlayerOrderIndex, localPlayerOrderIndex);

        return rotatedPlayerIds;
    }
}

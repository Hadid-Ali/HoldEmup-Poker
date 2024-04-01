using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Seat
{
    public int Number;
    public NetworkPlayer Player;

    public bool IsOccupied;
}
public class PlayerSeats : MonoBehaviour
{
     public const int MaxSeats = 4;
     
     [SerializeField] private Seat[] seats = new Seat[MaxSeats];
     public Dictionary<int, NetworkPlayer> ActivePlayers = new();

     public int localPlayerID => ActivePlayers.FirstOrDefault(x=> x.Value.IsLocalPlayer).Key;
     

     private void Awake()
     {
         //Initialize Seats
         for (int i = 0; i < seats.Length; i++)
             seats[i] = new Seat(){IsOccupied = false};
         
     }

     private void Start()
     {
         NetworkPlayer.OnPlayerSpawn += AssignSeat;

         StartCoroutine(WaitRoutine());
     }

     IEnumerator WaitRoutine()
     {
         yield return new WaitForSeconds(3f);
         
         TurnSequenceHandler.TurnViewSequence = RotateOrder(TurnSequenceHandler.TurnSequence);
         GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.Raise(TurnSequenceHandler.TurnViewSequence);
     }

     private void OnDestroy()
     {
         NetworkPlayer.OnPlayerSpawn -= AssignSeat;
     }

     private void AssignSeat(NetworkPlayer player)
     {
         for (int i = 0; i < seats.Length; i++)
         {
             if (seats[i].IsOccupied)
                 continue;
             
             seats[i].Number = i;
             seats[i].Player = player;
             seats[i].IsOccupied = true;
             
             player.SyncInformationGlobally();
             ActivePlayers.Add(player.id, player);
             TurnSequenceHandler.TurnSequence[i] = player.id;
             break;
         }
     }
     int[] RotateOrder(int[] turnSequence)
     {
         int localPlayerIndex = Array.IndexOf(turnSequence, localPlayerID);
         int[] rotatedPlayerIds = new int[turnSequence.Length];
        
         for (int i = 0; i < turnSequence.Length; i++)
         {
             int turnIndex = (localPlayerIndex + i) % turnSequence.Length;
             int playerId = turnSequence[turnIndex];
             rotatedPlayerIds[i] = playerId;
         }
        
         int localPlayerOrderIndex = Array.IndexOf(rotatedPlayerIds, localPlayerID);
        
         Array.Copy(rotatedPlayerIds, localPlayerOrderIndex, rotatedPlayerIds, 0, turnSequence.Length - localPlayerOrderIndex);
         Array.Copy(rotatedPlayerIds, 0, rotatedPlayerIds, turnSequence.Length - localPlayerOrderIndex, localPlayerOrderIndex);

         return rotatedPlayerIds;
     }
}
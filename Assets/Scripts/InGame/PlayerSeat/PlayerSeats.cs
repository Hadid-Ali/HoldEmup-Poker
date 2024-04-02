using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
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
    [SerializeField] private TurnSequenceHandler turnSequenceHandler;
    
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
     private void OnDestroy()
     {
         NetworkPlayer.OnPlayerSpawn -= AssignSeat;
     }

     IEnumerator WaitRoutine()
     {
         yield return new WaitForSeconds(4f);

         foreach (var v in ActivePlayers)
            v.Value.SyncInformationGlobally();
         
         turnSequenceHandler.TurnViewSequence = RotateOrder(turnSequenceHandler.TurnSequence);
         GameEvents.NetworkGameplayEvents.OnAllPlayersSeated.Raise(turnSequenceHandler.TurnViewSequence);
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
             
             ActivePlayers.Add(player.id, player);
             turnSequenceHandler.TurnSequence.Add(player.id);
             break;
         }
     }
     List<int> RotateOrder(List<int> turnSequence)
     {
         if (!turnSequence.Contains(localPlayerID))
         {
             Debug.LogError("Local player ID not found in turn sequence.");
             return null;
         }

         int localPlayerIndex = turnSequence.IndexOf(localPlayerID);
         List<int> rotatedPlayerIds = new List<int>();
         
         for (int i = 0; i < turnSequence.Count; i++)
         {
             int turnIndex = (localPlayerIndex + i) % turnSequence.Count;
             int playerId = turnSequence[turnIndex];
             rotatedPlayerIds.Add(playerId);
         }
         
         int localPlayerOrderIndex = rotatedPlayerIds.IndexOf(localPlayerID);
         
         rotatedPlayerIds.InsertRange(0, rotatedPlayerIds.GetRange(turnSequence.Count - localPlayerOrderIndex, localPlayerOrderIndex));
         rotatedPlayerIds.RemoveRange(turnSequence.Count, localPlayerOrderIndex);

         return rotatedPlayerIds;
     }
}
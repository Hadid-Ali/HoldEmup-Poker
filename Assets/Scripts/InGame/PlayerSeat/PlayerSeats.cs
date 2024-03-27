using System;
using System.Collections.Generic;
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
     public static PlayerSeats Instance { get; private set; }
     private const int MaxSeats = 5;
     
     [SerializeField] private Seat[] seats = new Seat[MaxSeats];
     
     public List<NetworkPlayer> activePlayers = new();
     

     private void Awake()
     {
         if(Instance == null)
            Instance = this;
         else
            Destroy(gameObject);

         //Initialize Seats
         for (int i = 0; i < seats.Length; i++)
             seats[i] = new Seat(){IsOccupied = false};
     }

     private void Start()
     {
         NetworkPlayer.OnPlayerSpawn += AssignSeat;
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
             
             activePlayers.Add(player);
             TurnSequenceHandler.TurnSequence[i] = player.id;
             break;
         }
         
     }
}
using System.Collections.Generic;
using UnityEngine;

public class TurnSequenceHandler : MonoBehaviour
{
    [SerializeField] private PlayerSeats _playerSeats;
    
    [SerializeField] public List<int> TurnSequence = new();
    [SerializeField] public List<int> TurnViewSequence = new();

    public static int currentTurnIndex;


    public NetworkPlayer CurrentTurnPlayer => _playerSeats.ActivePlayers[currentTurnIndex];

    public void RotateTurn()
    {
        if(currentTurnIndex >= TurnSequence.Count)
            return; 
        
        currentTurnIndex++;
    }

}

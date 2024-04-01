using UnityEngine;

public class TurnSequenceHandler : MonoBehaviour
{
    [SerializeField] private PlayerSeats _playerSeats;
    
    public static int[] TurnSequence = new int[PlayerSeats.MaxSeats];
    public static int[] TurnViewSequence = new int[PlayerSeats.MaxSeats];
    public static int[] PreflopTurnSequence;

    public static int currentTurnIndex;


    public NetworkPlayer CurrentTurnPlayer => _playerSeats.ActivePlayers[currentTurnIndex];

    public void RotateTurn()
    {
        if(currentTurnIndex >= TurnSequence.Length)
            return; 
        
        currentTurnIndex++;
    }

}

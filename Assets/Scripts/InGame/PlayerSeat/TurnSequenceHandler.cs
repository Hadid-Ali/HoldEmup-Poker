using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnSequenceHandler : MonoBehaviour
{
    [SerializeField] public List<int> TurnSequence = new();
    [SerializeField] public List<int> TurnViewSequence = new();

    private int _currentTurnIndex;

    public int CurrentTurnIndex
    {
        set => _currentTurnIndex = CurrentTurnIndex >= TurnSequence.Count - 1 ? 0 : value;
        get => _currentTurnIndex;
    }


}

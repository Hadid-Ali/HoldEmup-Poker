using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnSequenceHandler : MonoBehaviour
{
    [SerializeField] public List<int> TurnSequence = new();
    [SerializeField] public List<int> TurnViewSequence = new();

    private int _currentTurnIndex;

    private void Awake()
    {
        GameEvents.NetworkGameplayEvents.OnRoundEnd.Register(RotateSequence);
    }

    private void OnDestroy()
    {
        GameEvents.NetworkGameplayEvents.OnRoundEnd.UnRegister(RotateSequence);
    }

    public void RotateSequence()
    {
        (TurnSequence[0], TurnSequence[^1]) = (TurnSequence[^1], TurnSequence[0]);
    }
    public int CurrentTurnIndex
    {
        set => _currentTurnIndex = CurrentTurnIndex >= TurnSequence.Count - 1 ? 0 : value;
        get => _currentTurnIndex;
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkFlowHandler : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.NetworkEvents.PlayersJoined.Register(OnPlayersJoined);
    }

    private void OnDisable()
    {
        GameEvents.NetworkEvents.PlayersJoined.UnRegister(OnPlayersJoined);
    }

    protected abstract void OnPlayersJoined();
}

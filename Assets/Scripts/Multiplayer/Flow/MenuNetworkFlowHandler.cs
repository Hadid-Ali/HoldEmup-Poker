using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MenuNetworkFlowHandler : NetworkFlowHandler
{
    private void Start()
    {
        PhotonNetwork.Disconnect();
    }

    protected override void OnPlayersJoined()
    {
        NetworkManager.Instance.LoadGameplay();
    }
}

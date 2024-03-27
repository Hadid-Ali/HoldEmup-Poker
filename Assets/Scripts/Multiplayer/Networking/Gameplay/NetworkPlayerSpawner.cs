using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Serialization;

public class NetworkPlayerSpawner : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(SpawnPlayer), 1f);
    }
    
    public void SpawnPlayer()
    { 
        NetworkPlayer player =  PhotonNetwork.Instantiate($"Network/Player/Avatars/PlayerAvatar", Vector3.zero,
            Quaternion.identity, 0).GetComponent<NetworkPlayer>();
    }
    
}

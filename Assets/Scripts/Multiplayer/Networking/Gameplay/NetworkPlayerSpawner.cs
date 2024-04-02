
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


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

        player.PhotonPlayer = PhotonNetwork.LocalPlayer;
        player.nickName = player.PhotonPlayer.NickName;
        player.id = player.PhotonPlayer.ActorNumber;

    }
    
}

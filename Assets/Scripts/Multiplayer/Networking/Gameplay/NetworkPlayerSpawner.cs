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

        Player p = PhotonNetwork.LocalPlayer;
        player.nickName = p.NickName;
        player.id = p.ActorNumber;
        
        player.SyncInformationGlobally();
    }
    
}

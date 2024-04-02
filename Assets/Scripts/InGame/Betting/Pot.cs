using System;
using Photon.Pun;
using UnityEngine;

public class Pot : MonoBehaviour
{
     [SerializeField] private PhotonView _photonView;
     
     private int _money;
     public int GetPotMoney => _money;

     public static Action<int> OnPotUpdate;
     
     public void AddToPot(int val)
     {
          _money += val;
          _photonView.RPC(nameof(SyncPot_RPC), RpcTarget.All, _money);
          print("Add to Pot local");
     }

     [PunRPC]
     public void SyncPot_RPC(int val)
     {
          _money = val;
          OnPotUpdate?.Invoke(val);
          print("Add to Pot Broadcast");
     }

}
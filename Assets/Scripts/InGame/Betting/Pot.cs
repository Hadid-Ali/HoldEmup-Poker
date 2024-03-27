using System;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;

public class Pot : MonoBehaviour
{
     [SerializeField] private PhotonView _photonView;
     
     private int _money;
     public int GetPotMoney => _money;

     public static Action<int> OnPotUpdate;
     
     public void AddTopPot(int val)
     {
          if(!PhotonNetwork.IsMasterClient)
               return;
       
          _money += val;
          
          _photonView.RPC(nameof(SyncPot_RPC), RpcTarget.Others, val);
     }

     [PunRPC]
     public void SyncPot_RPC(int val)
     {
          _money = val;
          OnPotUpdate?.Invoke(val);
     }

}
using System;
using Photon.Pun;
using UnityEngine;

public class Pot : MonoBehaviour
{
     [SerializeField] private PhotonView _photonView;
     
     private int _money;
     public int GetPotMoney => _money;

     public static Action<int> OnPotUpdate;

     private void Awake()
     {
          GameEvents.NetworkGameplayEvents.OnRoundEnd.Register(ResetPot);
     }

     private void OnDestroy()
     {
          GameEvents.NetworkGameplayEvents.OnRoundEnd.UnRegister(ResetPot);
     }

     public void AddToPot(int val)
     {
          _photonView.RPC(nameof(SyncPot_RPC), RpcTarget.All, val);
     }

     public void ResetPot()
     {
          _photonView.RPC(nameof(Reset_RPC), RpcTarget.All);
     }

     [PunRPC]
     private void Reset_RPC()
     {
          _money = 0;
          OnPotUpdate?.Invoke(_money);
     }
     [PunRPC]
     public void SyncPot_RPC(int val)
     {
          _money += val;
          OnPotUpdate?.Invoke(_money);
     }

}
using Photon.Pun;
using UnityEngine;

public class PlayerCredit : MonoBehaviour
{
   [SerializeField] private PhotonView photonView;
   [SerializeField] private NetworkPlayer player;

   public bool IsBroke => Credits <= 0;
   public int Credits { get; private set; } = 1000;

   public void SubCredit(int val)
   {
      if(val > Credits)
         return;
        
      Credits -= val;  
      photonView.RPC(nameof(SyncInformation), RpcTarget.All, Credits);
   }

   public void AddCredit(int val)
   {
      Credits += val;  
      photonView.RPC(nameof(SyncInformation), RpcTarget.All, Credits);
   }

   #region RPC

   [PunRPC]
   private void SyncInformation(int credits)
   {
      Credits = credits;
        
      GameEvents.NetworkPlayerEvents.OnPlayerCreditsChanged.Raise(player.id,credits);
   }

   #endregion
   
}

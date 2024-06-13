using Photon.Pun;
using UnityEngine;

public class PlayerCredit : MonoBehaviour
{
   [SerializeField] private PhotonView photonView;
   [SerializeField] private NetworkPlayer player;

   public bool IsBroke => Credits <= 0;
   [field: SerializeField] public int Credits { get; set; }

   public void SubCredit(int val)
   {
      Credits = val > Credits? 0 : Credits -= val;
      photonView.RPC(nameof(SyncInformation), RpcTarget.All, player.id, Credits);
   }

   public void AddCredit(int val)
   {
      Credits += val;  
      photonView.RPC(nameof(SyncInformation), RpcTarget.All,player.id, Credits);
   }

   #region RPC

   [PunRPC]
   private void SyncInformation(int id, int credits)
   {
      if (player.id == id)
      {
         Credits = credits; 
         GameEvents.NetworkPlayerEvents.OnPlayerCreditsChanged.Raise(player.id,credits);
         
         print($"{player.id}-ID : {Credits} Changed");
      }
   }

   #endregion
   
}

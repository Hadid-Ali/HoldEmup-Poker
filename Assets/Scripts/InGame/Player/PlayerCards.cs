using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCards : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private NetworkPlayer player;
    
    [field:SerializeField]private List<CardData> PocketCards = new(Constants.Player.PocketCardLimit);

    private void Awake() => 
        GameEvents.NetworkGameplayEvents.OnShowDown.Register(SubmitCards);
    private void OnDestroy() => 
        GameEvents.NetworkGameplayEvents.OnShowDown.UnRegister(SubmitCards);

    private void SubmitCards()
    {
        GameEvents.NetworkGameplayEvents.NetworkSubmitRequest.Raise(new NetworkDataObject(PocketCards, player.id));
        print("Cards Submitted Locally");
    }
    
    public void SetPocketCards(CardData card1, CardData card2)
    {
        int[] binaryData1 = card1.ConvertToIntArray();
        int[] binaryData2 = card2.ConvertToIntArray();
        
        photonView.RPC(nameof(SyncPocketCards_RPC), RpcTarget.All, binaryData1,binaryData2);
    }
    

    #region RPC
    
    [PunRPC]
    private void SyncPocketCards_RPC(int[] binaryCardData1, int[] binaryCardData2)
    {
        PocketCards.Clear();

        PocketCards.Add(CardData.ConvertIntArrayToCardData(binaryCardData1));
        PocketCards.Add(CardData.ConvertIntArrayToCardData(binaryCardData2));
            
        if(player.IsLocalPlayer) 
            GameEvents.NetworkPlayerEvents.ExposePocketCardsLocally.Raise(PocketCards);    
    }

    
    #endregion
    
}

using Photon.Pun;

public class MenuNetworkFlowHandler : NetworkFlowHandler
{
    private void Start()
    {
        PhotonNetwork.Disconnect();
    }

    private void OnDisconnect()
    {
        
    }

    protected override void OnPlayersJoined()
    {
        //NetworkManager.Instance.LoadGameplay();
    }
}

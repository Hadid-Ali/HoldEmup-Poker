using Photon.Pun;

public partial class NetworkManager : MonobehaviourSingleton<NetworkManager>
{
    public static class NetworkUtilities
    {
        public static void RaiseRPC(PhotonView view, string methodName, RpcTarget rpcTarget, object[] RPC_Params)
        {
            view.RPC(methodName, rpcTarget, RPC_Params);
        }
    }
}
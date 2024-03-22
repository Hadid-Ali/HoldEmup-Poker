using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DisconnectionHandler : MonoBehaviourPunCallbacks
{
    private bool m_HasDisconnected = false;
    
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // App is in the background
            // Handle background pause (e.g., disconnect Photon)
            PhotonNetwork.Disconnect();
        }
        else
        {
            // App is in the foreground
            // Reconnect or handle resume
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            if (!m_HasDisconnected)
                return;
            
            PhotonNetwork.ConnectUsingSettings();
            m_HasDisconnected = false;
        }
        else
        {
            // App lost focus
            // Handle background pause (e.g., disconnect Photon)

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
                m_HasDisconnected = true;
            }
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // Handle disconnection here
        Debug.LogWarning($"OnDisconnected: {cause.ToString()}");
    }
}
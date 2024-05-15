using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkEventsListener : MonoBehaviour
{
   [SerializeField] private ConnectionController m_ConnectionController;
   [SerializeField] private PhotonView m_NetworkEventsListenerComponent;

   private void OnEnable()
   {
      GameEvents.NetworkEvents.NetworkTimerStartRequest.Register(OnNetworkTimerStartRequest);
      GameEvents.GameFlowEvents.LeaveMatch.Register(OnLeaveMatch);
   }

   private void OnDisable()
   {
      GameEvents.NetworkEvents.NetworkTimerStartRequest.UnRegister(OnNetworkTimerStartRequest);
      GameEvents.GameFlowEvents.LeaveMatch.UnRegister(OnLeaveMatch);
   }

   private void OnLeaveMatch()
   {
     // m_ConnectionController.Disconnect();
   }
   
   private void OnNetworkTimerStartRequest(string message, float timerAmount)
   {
      NetworkManager.NetworkUtilities.RaiseRPC(m_NetworkEventsListenerComponent, nameof(OnNetworkTimerStartRequest_RPC),
         RpcTarget.All, new object[]
         {
            message, timerAmount
         });
   }

   [PunRPC]
   private void OnNetworkTimerStartRequest_RPC(string message, float timerAmount)
   {
      GameEvents.MenuEvents.TimeBasedActionRequested.Raise(message, timerAmount);
   }
}

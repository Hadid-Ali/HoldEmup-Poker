using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class ConnectionControllerView : MonoBehaviour
{
    [SerializeField] private ConnectionController m_ConnectionController;

    private void OnEnable()
    {
        GameEvents.MenuEvents.LoginAtMenuEvent.Register(OnLogin);
        GameEvents.NetworkEvents.OnRegionSelect.Register(OnRegionSelection);
        GameEvents.NetworkEvents.OnRoomSelect.Register(OnCreateRoom);
    }

    private void OnDisable()
    {
        GameEvents.MenuEvents.LoginAtMenuEvent.UnRegister(OnLogin);
        GameEvents.NetworkEvents.OnRegionSelect.UnRegister(OnRegionSelection);
        GameEvents.NetworkEvents.OnRoomSelect.UnRegister(OnCreateRoom);
    }

    private void OnLogin(string userName)
    {
        m_ConnectionController.StartConnectionWithName(userName);
    }

    private void OnRegionSelection(Region region)
    {
        m_ConnectionController.OnRegionSelect(region);
    }

    private void OnCreateRoom(RoomOptions roomOptions)
    {
        m_ConnectionController.CreateRoom(roomOptions);
    }
}

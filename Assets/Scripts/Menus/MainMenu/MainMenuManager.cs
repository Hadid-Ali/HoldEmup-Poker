
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class MainMenuManager : MenusController
{
    [SerializeField] private VoidEvent m_RoomJoinFailedEvent;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_RoomJoinFailedEvent.Register(ShowRoomCreationScreen);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_RoomJoinFailedEvent.Unregister(ShowRoomCreationScreen);
    }
    
    private void ShowRoomCreationScreen()
    {
       // SetMenuState(MenuName.CreateRoom);
    }
}

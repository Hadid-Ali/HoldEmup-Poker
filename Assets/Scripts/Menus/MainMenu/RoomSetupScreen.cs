using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSetupScreen : UIMenuBase
{
   [SerializeField] private TMP_InputField m_RoomsizeField;
   [SerializeField] private Button m_CreateRoomButton;

   [SerializeField] private PhotonRoomCreationEvent m_RoomCreationEvent;

   private int m_Roomsize = 2;

   private void Start()
   {
      m_CreateRoomButton.onClick.AddListener(CreateRoom);
      m_RoomsizeField.onValueChanged.AddListener(OnRoomSizeValueChanged);
      CreateRoom();
   }

   protected override void OnContainerEnable()
   {
      base.OnContainerEnable();
      Invoke(nameof(CreateRoom), 0.5f);
   }

   private void CreateRoom()
   {
      m_RoomCreationEvent.Raise(new RoomOptions()
      {
         MaxPlayers = GameData.MetaData.MaxPlayersLimit,
         IsOpen = true,
         IsVisible = true,
         PlayerTtl = -1,
      });
      GameData.SessionData.CurrentRoomPlayersCount = GameData.MetaData.MaxPlayersLimit;
      ChangeMenuState(MenuName.ConnectionScreen);
   }

   private void OnRoomSizeValueChanged(string text)
   {
      if (int.TryParse(text,out m_Roomsize))
      {
         if (m_Roomsize > 1)
         {
            m_CreateRoomButton.interactable = true;
            return;
         }
      }
      m_CreateRoomButton.interactable = false;
   }
}

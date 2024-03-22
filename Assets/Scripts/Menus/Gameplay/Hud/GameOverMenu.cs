using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : UIMenuBase
{
   [SerializeField] private ButtonWidget m_GoToMenuButton;

   private void Start()
   {
      m_GoToMenuButton.SubscribeAction(OnGoToMenuClick);
   }

   protected override void OnContainerEnable()
   {
      base.OnContainerEnable();
      GameEvents.GameplayEvents.GameplayStateSwitched.Raise(GameplayState.Casino_View);
   }

   //TODO: Implement Scene Flow Controller
   private void OnGoToMenuClick()
   {
      GameEvents.GameFlowEvents.LeaveMatch.Raise();
      SceneManager.LoadScene("GameMenu");
   }
}

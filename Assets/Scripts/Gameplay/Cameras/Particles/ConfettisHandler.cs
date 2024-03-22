using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettisHandler : MonoBehaviour
{
   [SerializeField] private GameObject m_ParticlesContainer;
   
   private bool m_IsWinner = false;
   
   private void OnEnable()
   {
      GameEvents.NetworkGameplayEvents.MatchWinnersAnnounced.Register(OnWinnerAnnounced);
      GameEvents.GameplayUIEvents.MatchCompleteMenuShown.Register(OnMatchCompleteMenuShown);
   }

   private void OnDisable()
   {
      GameEvents.NetworkGameplayEvents.MatchWinnersAnnounced.UnRegister(OnWinnerAnnounced);
      GameEvents.GameplayUIEvents.MatchCompleteMenuShown.UnRegister(OnMatchCompleteMenuShown);
   }

   private void OnWinnerAnnounced(List<int> winners, bool isWinner)
   {
      m_IsWinner = isWinner;
   }

   private void OnMatchCompleteMenuShown()
   {
      m_ParticlesContainer.SetActive(m_IsWinner);
   }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerasContainer : MonoBehaviour
{
  [SerializeField] private GameObject m_CasinoCamera;
  [SerializeField] private GameObject m_CardsCamera;

  private void OnEnable()
  {
    GameEvents.GameplayEvents.GameplayStateSwitched.Register(OnGameplayStateSwitched);
  }

  private void OnDisable()
  {
    GameEvents.GameplayEvents.GameplayStateSwitched.UnRegister(OnGameplayStateSwitched);
  }

  private void OnGameplayStateSwitched(GameplayState gameplayState)
  {
    m_CasinoCamera.SetActive(gameplayState is GameplayState.Casino_View);
    m_CardsCamera.SetActive(gameplayState is GameplayState.Cards_View);
  }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DemoGameUI : MonoBehaviour
{
   [SerializeField] private GameplayHandTypeEvent m_OnHandUpdatedEvent;

   private void OnEnable()
   {
      m_OnHandUpdatedEvent.Register(OnHandTypeUpdateEvent);
   }

   private void OnDisable()
   {
      m_OnHandUpdatedEvent.Unregister(OnHandTypeUpdateEvent);
   }

   private void OnHandTypeUpdateEvent(HandTypes handType)
   {
     // m_CurrentHandTypeText.text = $"Hand Type <color=yellow>{handType}</color>";
   }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TimerController : MonoBehaviour
{
   private float m_TimeToWait = 0;

   private Coroutine m_RequestRoutine = null;
   private GameEvent m_OnTimerCompletedEvent = new();

   private void Start()
   {
      DontDestroyOnLoad(gameObject);
   }
   
   private void OnEnable()
   {
      GameEvents.TimerEvents.ExecuteActionRequest.Register(OnExecuteActionRequest);
      GameEvents.TimerEvents.CancelActionRequest.Register(OnRequestCancel);
   }

   private void OnDisable()
   {
      GameEvents.TimerEvents.ExecuteActionRequest.UnRegister(OnExecuteActionRequest);
      GameEvents.TimerEvents.CancelActionRequest.UnRegister(OnRequestCancel);
   }

   private void OnExecuteActionRequest(TimerDataObject timerDataObject)
   {
      float timeDuration = timerDataObject.TimeDuration;

      m_TimeToWait = timerDataObject.TimeDuration;
      InitializeEvent(timerDataObject.ActionToExecute);

      if (timerDataObject.IsNetworkGlobal)
         GameEvents.NetworkEvents.NetworkTimerStartRequest.Raise(timerDataObject.Title, timeDuration);

      m_RequestRoutine = StartCoroutine(EventRequestRoutine());
   }

   private void OnRequestCancel()
   {
      m_OnTimerCompletedEvent.UnRegisterAll();
      StopCoroutine(m_RequestRoutine);
      m_RequestRoutine = null;
   }
   
   private void InitializeEvent(Action action)
   {
      m_OnTimerCompletedEvent.UnRegisterAll();
      m_OnTimerCompletedEvent.Register(action);
   }

   private IEnumerator EventRequestRoutine()
   {
      yield return new WaitForSecondsRealtime(m_TimeToWait);
      m_OnTimerCompletedEvent.Raise();

      Debug.LogError($"Event Executed {m_TimeToWait}");
   }
}

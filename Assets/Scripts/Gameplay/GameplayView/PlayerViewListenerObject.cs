using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerViewListenerObject : MonoBehaviour
{
   protected virtual void OnEnable()
   {
      GameEvents.GameplayEvents.LocalPlayerJoined.Register(OnLocalPlayerJoined);
   }

   protected virtual void OnDisable()
   {
      GameEvents.GameplayEvents.LocalPlayerJoined.UnRegister(OnLocalPlayerJoined);
   }

   protected abstract void OnLocalPlayerJoined(PlayerViewDataObject viewDataObject);
}

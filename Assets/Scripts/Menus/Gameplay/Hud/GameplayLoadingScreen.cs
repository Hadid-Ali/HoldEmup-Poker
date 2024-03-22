using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayLoadingScreen : UIMenuBase
{
    private void OnEnable()
    {
        GameEvents.GameFlowEvents.RoundStart.Register(DisableMenu);
    }

    private void OnDisable()
    {
        GameEvents.GameFlowEvents.RoundStart.UnRegister(DisableMenu);
    }

    private void DisableMenu()
    {
        SetMenuActiveState(false);
    }
}

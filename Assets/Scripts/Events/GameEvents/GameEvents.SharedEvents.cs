using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GameEvents
{
    public static class SharedEvents
    {
        public static GameEvent<ViewMode> GameViewSwitched = new();
    }
}


using System;

public static partial class GameEvents
{
    public static class TimerEvents
    {
        public static GameEvent<TimerDataObject> ExecuteActionRequest = new();
        public static GameEvent CancelActionRequest = new();
    }
}

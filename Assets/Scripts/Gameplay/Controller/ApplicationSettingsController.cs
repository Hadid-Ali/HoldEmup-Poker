using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSettingsController : MonoBehaviour
{
    void Start()
    {
        SetFrameRate();
        SetScreenTime();
    }

    public void SetFrameRate()
    {
        Application.targetFrameRate = 60;
    }

    public void SetScreenTime()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}

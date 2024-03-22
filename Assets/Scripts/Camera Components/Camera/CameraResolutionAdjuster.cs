using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolutionAdjuster : MultiResObject
{
    [Header("Components")]
    [SerializeField] private List<CameraResolutionConfig> m_Cameras;

    protected override void OnDeviceModeInput(DeviceMode deviceMode)
    {
        for (int i = 0; i < m_Cameras.Count; i++)
        {
            DeviceMode mode = m_Cameras[i].ResolutionMode;
            m_Cameras[i].SetEnabled(mode == deviceMode);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MultiResObject : MonoBehaviour
{
    private DeviceMode m_DeviceMode;

    protected virtual void Start()
    {
        CheckDeviceMode();
    }

    private void CheckDeviceMode()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        m_DeviceMode = screenRatio >= 1.5f ? DeviceMode.Phone : DeviceMode.Tab;

        OnDeviceModeInput(m_DeviceMode);
    }

    protected abstract void OnDeviceModeInput(DeviceMode deviceMode);
}

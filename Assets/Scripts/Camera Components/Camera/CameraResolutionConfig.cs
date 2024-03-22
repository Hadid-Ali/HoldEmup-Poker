using System;
using UnityEngine;

[Serializable]
public class CameraResolutionConfig
{
    [field: SerializeField] public DeviceMode ResolutionMode { get; private set; }
    [field: SerializeField] public GameObject CamerasContainer;

    public void SetEnabled(bool status)
    {
        CamerasContainer.SetActive(status);
    }
}

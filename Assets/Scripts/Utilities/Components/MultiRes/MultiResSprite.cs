using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiResSprite : MultiResObject
{
    [SerializeField] private List<MultiResTransformObject> m_ObjectScales = new();
    [SerializeField] private Transform m_Transform;

    protected override void Start()
    {
        m_Transform = transform;
        base.Start();
    }

    protected override void OnDeviceModeInput(DeviceMode deviceMode)
    {
        m_Transform ??= transform;
        
        MultiResTransformObject transformObject =
            m_ObjectScales.Find(scaleObject => scaleObject.ResolutionMode == deviceMode);
        
        m_Transform.localScale = transformObject.Scale;
        m_Transform.position = transformObject.Position;
    }
}

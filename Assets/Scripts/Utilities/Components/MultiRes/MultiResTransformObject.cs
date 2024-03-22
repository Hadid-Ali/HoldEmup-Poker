using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MultiResTransformObject
{
    [field: SerializeField] public DeviceMode ResolutionMode { get; private set; }
    
    [field: SerializeField] public Vector3 Position { get; private set; }
    [field: SerializeField] public Vector3 Scale { get; private set; }
}

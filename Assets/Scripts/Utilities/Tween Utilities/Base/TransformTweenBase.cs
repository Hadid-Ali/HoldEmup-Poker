using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransformTweenBase : BaseTween
{
    [SerializeField] protected Transform m_Transform;
    [SerializeField] protected bool m_DoLocalTween;
    
    protected override void Init()
    {
        m_Transform ??= transform;
    }
}

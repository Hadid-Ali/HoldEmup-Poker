using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovementTweenUtility : TransformTweenBase
{
    public override void PerformTweenOperation()
    {
        if (m_DoLocalTween)
        {

            m_Transform.DOLocalMove(m_TweenData.Target, SequenceDuration());
        }
        else
        {
         
            m_Transform.DOMove(m_TweenData.Target, SequenceDuration());
        }
    }
}

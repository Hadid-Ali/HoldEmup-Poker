using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasinoViewObject : MonoBehaviour
{
    [SerializeField] private Animator m_CasinoViewAnimator;

    [SerializeField] private GameObject m_ContainerObject;
    
    private int m_ScaleInParamter => Animator.StringToHash("ScaleIn");
    private int m_ScaleOutParamter => Animator.StringToHash("ScaleOut");

    public void SetViewState(bool state)
    {
        if (state)
            ResetScale();
        
        m_ContainerObject.SetActive(state);
    }

    private void ResetScale()
    {
        m_ContainerObject.transform.localScale = Vector3.one;
        m_ContainerObject.transform.localPosition = Vector3.zero;
    }

    public void SetCasinoViewAnimatorState(bool scaleOut)
    {
        m_CasinoViewAnimator.SetTrigger(scaleOut ? m_ScaleOutParamter : m_ScaleInParamter);
    }

}

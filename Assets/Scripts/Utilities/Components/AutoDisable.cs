using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    [SerializeField] private float m_DisableTime = 2f;
    [SerializeField] private bool m_ShouldDestroy = false;

    private void OnEnable()
    {
        Invoke(nameof(Disable), m_DisableTime);
    }

    private void Disable()
    {
        if(m_ShouldDestroy)
            Destroy(gameObject);
        else 
            gameObject.SetActive(false);
    }
}

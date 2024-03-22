using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform m_TargetToLook;
    [SerializeField] private float m_YMultiplier = 1f;
    
    private Transform m_Transform;
    private Vector3 m_MultiplierVector;
    
    private void Start()
    {
        m_TargetToLook = Camera.main.transform;
        m_Transform = transform;

        m_MultiplierVector = new Vector3(1f, m_YMultiplier, 1f);
    }

    private void FixedUpdate()
    {
        if (m_TargetToLook == null)
            return;
        
        m_Transform.LookAt(m_TargetToLook);
        Vector3 v = m_Transform.eulerAngles;

        m_Transform.eulerAngles = new Vector3(v.x, v.y * m_YMultiplier, v.z);
    }
}

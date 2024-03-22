using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayRefs : MonoBehaviour,IGameplayRefs
{
    [SerializeField] private Camera m_MainCamera;
    
    public Vector2 GetWorldToScreenPoint(Vector3 position) => m_MainCamera.WorldToScreenPoint(position);

    private void Awake()
    {
        Dependencies.GameplayRefs = this;
    }
}

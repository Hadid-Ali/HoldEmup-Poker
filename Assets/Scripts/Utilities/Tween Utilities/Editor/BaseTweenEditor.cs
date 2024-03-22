using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseTween),editorForChildClasses:true)]
public class BaseTweenEditor : Editor
{
    private BaseTween m_TweenComponent;

    private void OnEnable()
    {
        m_TweenComponent = (BaseTween)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Play Tween"))
        {
            m_TweenComponent.PerformTweenOperation();
        }
    }
}

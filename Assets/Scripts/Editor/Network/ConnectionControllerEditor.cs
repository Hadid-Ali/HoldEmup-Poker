using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConnectionController))]
public class ConnectionControllerEditor : Editor
{
    private ConnectionController m_ConnectionController;

    private void OnEnable()
    {
        m_ConnectionController ??= (ConnectionController)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Disconnect"))
        {
            //m_ConnectionController.Disconnect();   
        } 
        
        if (GUILayout.Button("Start Match"))
        {
           // m_ConnectionController.StartMatch();  
        } 
    }
}

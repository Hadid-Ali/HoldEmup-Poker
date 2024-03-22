using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardDataObject))]
public class CardDataObjectEditor : Editor
{
    private CardDataObject m_CardDataObject;

    private void OnEnable()
    {
        m_CardDataObject = (CardDataObject)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Rename"))
        {
            string type = m_CardDataObject.type.ToString().Replace("TYPE_", "");
            string value = m_CardDataObject.value.ToString().Replace("VALUE_", "");
            
            RenameIDObject($"Card_{type}_{value}");
        }
    }
    
    public void RenameIDObject(string name)
    {
        string assetPath = AssetDatabase.GetAssetPath(m_CardDataObject.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, name);
        MarkDirty();
    }

    private void MarkDirty()
    {
        EditorUtility.SetDirty(m_CardDataObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorWindow : UIMenuBase
{
    [SerializeField] private TextMeshProUGUI m_ErrorText;
    
    private void OnEnable()
    {
        GameEvents.GameplayUIEvents.ErrorOccured.Register(OnErrorOccured);
    }

    private void OnDisable()
    {
        GameEvents.GameplayUIEvents.ErrorOccured.UnRegister(OnErrorOccured);
    }

    void OnErrorOccured(string errorString)
    {
        m_ErrorText.text = errorString;
        ChangeMenuState(MenuName.ErrorWindow);
    }
}

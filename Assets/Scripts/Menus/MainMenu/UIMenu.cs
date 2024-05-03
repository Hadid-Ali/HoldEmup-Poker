using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : UIMenuBase
{
    [SerializeField] private ButtonWidget m_PlayButton;

    private void Start()
    {
        m_PlayButton.SubscribeAction(LoginBtnEvent);
    }

    private void OnSettingsButton()
    {
        ChangeMenuState(MenuName.SettingsMenu);
    }
    
    public void LoginBtnEvent()
    {
        ChangeMenuState(MenuName.LoginScreen);
    }
}

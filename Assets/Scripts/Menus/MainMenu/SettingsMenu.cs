using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : UIMenuBase
{
    [SerializeField] private ButtonWidget m_PrivacyPolicyButton;
    [SerializeField] private ButtonWidget m_TermsOfUsageButton;
    [SerializeField] private ButtonWidget m_BackButton;
    [SerializeField] private ButtonWidget m_CloseButton;

    private void Start()
    {
        m_PrivacyPolicyButton.SubscribeAction(OnPrivacyPolicyButton);
        m_TermsOfUsageButton.SubscribeAction(OnTermsOfUsageButton);
        m_BackButton.SubscribeAction(OnBackButton);
        m_CloseButton.SubscribeAction(OnCloseButton);
    }

    private void OnPrivacyPolicyButton()
    {
        Application.OpenURL(GameData.MetaData.PrivacyPolicyLink);
    }

    private void OnTermsOfUsageButton()
    {
        Application.OpenURL(GameData.MetaData.TermsOfUsageLink);
    }
    
    private void OnBackButton()
    {
        ChangeMenuState(MenuName.MainMenu);
    }

    private void OnCloseButton()
    {
        OnBackButton();
    }
}

using UnityEngine;

public class UIMenu : UIMenuBase
{
    [SerializeField] private ButtonWidget m_PlayButton;

    private void Start()
    {
        m_PlayButton.SubscribeAction(LoginBtnEvent);
    }
    public void LoginBtnEvent()
    {
        ChangeMenuState(MenuName.LoginScreen);
    }
}

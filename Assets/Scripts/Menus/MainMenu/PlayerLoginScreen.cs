using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoginScreen : UIMenuBase
{
    [SerializeField] private TMP_InputField m_InputField;
    [SerializeField] private Button m_LoginButton;
    
    private void Start()
    {
        m_InputField.onValueChanged.AddListener(OnFieldValueChange);
        m_LoginButton.onClick.AddListener(OnLoginBtnEvent);

        SetButtonInteractionStatus(false);
        
        OnFieldValueChange(string.Empty);
        m_InputField.characterLimit = GameData.MetaData.MaximumNameLength;
    }

    protected override void OnContainerEnable()
    {
        base.OnContainerEnable();
        CheckForPreviousLogin();
    }

    private void OnFieldValueChange(string value)
    {
        bool hasValidLenght = value.Length >= GameData.MetaData.MinimumNameLength;
        SetButtonInteractionStatus(!string.IsNullOrEmpty(value) && hasValidLenght);
    }

    private void CheckForPreviousLogin()
    {
        if (GameData.RuntimeData.IS_LOGGED_IN)
        {
            LoginInternal();
        }
    }
    
    public void OnLoginBtnEvent()
    {
        string userName = m_InputField.text;
        
        GameData.RuntimeData.USER_NAME = userName;
        GameData.RuntimeData.IS_LOGGED_IN = true;
        LoginInternal();
    }

    private void LoginInternal()
    {
        GameEvents.NetworkEvents.PlayerLogin.Raise(GameData.RuntimeData.USER_NAME);
        ChangeMenuState(MenuName.ConnectionScreen);
    }

    private void SetButtonInteractionStatus(bool status)
    {
        m_LoginButton.interactable = status;
    }
}

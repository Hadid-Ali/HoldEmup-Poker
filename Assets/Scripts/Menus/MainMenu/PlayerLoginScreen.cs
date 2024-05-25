using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerLoginScreen : UIMenuBase
{
    [SerializeField] private TMP_InputField m_InputField;
    [SerializeField] private Button m_LoginButton;

    [System.Serializable]
    public class PlayerAccountCreationRequest
    {
        public string username;
    }

    [System.Serializable]
    public class PlayerAccountCreationResponse
    {
        public ulong account_id;
    }
    
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
        // create player account /v1/player/create-account here
        PlayerAccountCreationRequest request = new PlayerAccountCreationRequest
        {
            username = GameData.RuntimeData.USER_NAME
        };
        StartCoroutine(CreatePlayerAccount(request));

        GameEvents.MenuEvents.LoginAtMenuEvent.Raise(GameData.RuntimeData.USER_NAME);
        ChangeMenuState(MenuName.ConnectionScreen);
    }

    private IEnumerator CreatePlayerAccount(PlayerAccountCreationRequest request)
    {
        Debug.Log("Creating player account...");
        string url = "http://127.0.0.1:8000/v1/player/create-account";
        string jsonData = JsonUtility.ToJson(request);

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Sending account creation request...");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string responseText = webRequest.downloadHandler.text;
                PlayerAccountCreationResponse response = JsonUtility.FromJson<PlayerAccountCreationResponse>(responseText);
                Debug.Log("Account ID: " + response.account_id);
            }
        }
    }

    private void SetButtonInteractionStatus(bool status)
    {
        m_LoginButton.interactable = status;
    }
}

using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ConnectionControllerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_MatchStartTimerComponent;
    
    private GameEvent<string> m_OnLogin = new();
    private GameEvent<Region> m_OnRegionSelect = new();
    private GameEvent<RoomOptions> m_OnCreateRoom = new();
    private GameEvent m_OnCharacterSelected = new();

    private void OnEnable()
    {
        GameEvents.NetworkEvents.PlayerLogin.Register(OnLogin);
        GameEvents.NetworkEvents.PlayerRegionSelect.Register(OnRegionSelection);
        GameEvents.NetworkEvents.PlayerRoomCreation.Register(OnCreateRoom);
        GameEvents.NetworkEvents.PlayerCharacterSelected.Register(OnPlayerCharacterSelected);
    }

    private void OnDisable()
    {
        GameEvents.NetworkEvents.PlayerLogin.UnRegister(OnLogin);
        GameEvents.NetworkEvents.PlayerRegionSelect.UnRegister(OnRegionSelection);
        GameEvents.NetworkEvents.PlayerRoomCreation.UnRegister(OnCreateRoom);
        GameEvents.NetworkEvents.PlayerCharacterSelected.UnRegister(OnPlayerCharacterSelected);
    }
    
    public void Initialize(Action<string> onLogin, Action<Region> onRegionSelect, Action<RoomOptions> onCreateRoom,
        Action onCharacterSelect)
    {
        m_OnLogin.Register(onLogin);
        m_OnRegionSelect.Register(onRegionSelect);
        m_OnCreateRoom.Register(onCreateRoom);
        m_OnCharacterSelected.Register(onCharacterSelect);
    }

    public void SetTimerStatus(string timer)
    {
        // if (!m_MatchStartTimerComponent.gameObject.activeSelf)
        //     m_MatchStartTimerComponent.gameObject.SetActive(true);
        //
        // m_MatchStartTimerComponent.text = timer;
    }

    public void HideTimer()
    {
      //  m_MatchStartTimerComponent.gameObject.SetActive(false);
    }

    private void OnLogin(string userName)
    {
        m_OnLogin.Raise(userName);
    }

    private void OnRegionSelection(Region region)
    {
        m_OnRegionSelect.Raise(region);
    }

    private void OnCreateRoom(RoomOptions roomOptions)
    {
        m_OnCreateRoom.Raise(roomOptions);
    }

    private void OnPlayerCharacterSelected()
    {
        m_OnCharacterSelected.Raise();
    }
}

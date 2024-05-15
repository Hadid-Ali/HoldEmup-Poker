using UnityEngine;

public partial class NetworkManager : MonobehaviourSingleton<NetworkManager>
{
    [SerializeField] private NetworkSceneManager m_NetworkSceneManager;

    [field: SerializeField] public RegionsRegistry RegionsRegistry { get; private set; }
    
    [ContextMenu("Load Gameplay")]
    public void LoadGameplay()
    {
        m_NetworkSceneManager.LoadGameplayScene(1f);
    }

    public void SetStatus(string status)
    {
        GameEvents.NetworkEvents.NetworkStatus.Raise(status);
    }
}

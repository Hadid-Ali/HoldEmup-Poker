using UnityEngine;

public partial class NetworkManager : MonobehaviourSingleton<NetworkManager>
{
    [SerializeField] private NetworkSceneManager m_NetworkSceneManager;

    [field: SerializeField] public RegionsRegistry RegionsRegistry { get; private set; }
    
    [ContextMenu("Load Gameplay")]
    public void LoadGameplay(string sceneName)
    {
        m_NetworkSceneManager.LoadGameplayScene(sceneName,1f);
    }

    public void SetStatus(string status)
    {
        GameEvents.NetworkEvents.NetworkStatus.Raise(status);
    }
}


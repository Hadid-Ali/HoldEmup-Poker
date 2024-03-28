using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSceneManager : MonoBehaviour
{
    [SerializeField] private PhotonView m_PhotonView;

    private void Awake()
    {
        GameEvents.NetworkEvents.OnStartMatch.Register(LoadGameplayScene);
    }

    private void OnDestroy()
    {
        GameEvents.NetworkEvents.OnStartMatch.UnRegister(LoadGameplayScene);
    }

    [ContextMenu("Start Match")]
    public void LoadGameplayScene()
    {
        m_PhotonView.RPC(nameof(LoadGameplaySceneRPC), RpcTarget.All,1f);
        print("loading RPC call");
    }
    
    [PunRPC]
    private void LoadGameplaySceneRPC(float wait)
    {
        GameEvents.MenuEvents.NetworkStatusUpdated.Raise("\t\tLoading Game");
        StartCoroutine(LoadScene("PokerGame", wait));
        
        print("has Run");
    }

    //TODO: Implement Scene Flow Loader
    
    private IEnumerator LoadScene(string sceneName,float wait)
    {
     //   Debug.LogError("Load Scene");
        yield return new WaitForSeconds(wait);
        SceneManager.LoadSceneAsync(sceneName);
    }
}

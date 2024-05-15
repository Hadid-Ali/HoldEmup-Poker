using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSceneManager : MonoBehaviour
{
    public void LoadGameplayScene(float wait)
    {
        NetworkManager.Instance.SetStatus("Loading Game...");
        StartCoroutine(LoadScene("PokerGame", wait));
    }

    private IEnumerator LoadScene(string sceneName,float wait)
    {
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene(sceneName);
    }
}

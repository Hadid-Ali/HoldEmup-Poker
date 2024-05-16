using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkSceneManager : MonoBehaviour
{
    public void LoadGameplayScene(string sceneName,float wait)
    {
        NetworkManager.Instance.SetStatus("Loading Game...");
        StartCoroutine(LoadScene(sceneName, wait));
    }

    public void LoadMenuScene(float wait)
    {
        
    }

    private IEnumerator LoadScene(string sceneName,float wait)
    {
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene(sceneName);
    }
}

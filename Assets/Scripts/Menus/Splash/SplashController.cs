using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(LoadSceneInternal());
    }

    IEnumerator LoadSceneInternal()
    {
        yield return new WaitForSeconds(4f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameMenu");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
  
    }
}

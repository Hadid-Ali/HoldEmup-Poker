using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameExitButton : MonoBehaviour
{
    [SerializeField] private DialogBox box;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private Button button;
    [SerializeField] private GameObject loader;

    private void Awake()
    {
        button.onClick.AddListener(ExitGame);

        GameEvents.NetworkPlayerEvents.OnPlayerDisconnected.Register(OnPlayerLeaveMatch);
        GameEvents.NetworkPlayerEvents.OnPlayerDisconnected.Register(()=>photonView.RPC(nameof(OnPlayerLeftMatch), RpcTarget.All));
    }

    private void OnDestroy()
    {
        GameEvents.NetworkPlayerEvents.OnPlayerDisconnected.UnRegister(OnPlayerLeaveMatch);
        GameEvents.NetworkPlayerEvents.OnPlayerDisconnected.Register(()=>photonView.RPC(nameof(OnPlayerLeftMatch), RpcTarget.All));
    }

    public void ExitGame()
    {
        box.Initialize("Do you Really Want To Leave", OnPlayerLeaveMatch);
    }

    private void OnPlayerLeaveMatch()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        photonView.RPC(nameof(OnPlayerLeftMatch), RpcTarget.Others);
        yield return new WaitForSeconds(1f);
        LoadMenu();

    }

    private void LoadMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        loader.SetActive(true);
        SceneManager.LoadScene("GameMenu");
    }

    [PunRPC]
    private void OnPlayerLeftMatch()
    {
        box.Initialize("Some Player Have Left the match, Return to MainMenu", LoadMenu);
    }
}

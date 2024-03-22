using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PhotonView))]
public class NetworkPlayerController : PlayerController
{
    [SerializeField] protected PhotonView m_PhotonView;

    public override  bool IsLocalPlayer => m_PhotonView != null && m_PhotonView.IsMine;
    public override int ID => m_PhotonView.ViewID;
    public PhotonView NetworkView => m_PhotonView;
    
    
    protected override void Start()
    {
        IsBot = false;
        base.Start();
        OnSpawn();
    }

    public void OnSpawn()
    {
        
        PhotonNetwork.RegisterPhotonView(m_PhotonView);
        GameEvents.NetworkEvents.NetworkDisconnectedEvent.Register(OnNetworkDisconnect);
        GameEvents.GameplayEvents.RoundCompleted.Register(OnPlayerSubmitScore);
        
        //Assign ID before Event
        ID = m_PhotonView.ViewID;
        GameEvents.NetworkGameplayEvents.PlayerJoinedGame.Raise(this);
        
        
        if (!IsLocalPlayer)
            return;
        
        InitializeControls();
        SetPlayerDataOverServer();    
        
    }



    private void OnNetworkDisconnect()
    {
    }

    protected virtual void SetPlayerDataOverServer()
    {
        Player player = PhotonNetwork.LocalPlayer;
        
        string nickName = player.NickName;
        int actorNum = player.ActorNumber;
        
        Debug.LogError($"Player Data Set {actorNum} : {nickName}");
        
        NetworkManager.NetworkUtilities.RaiseRPC(m_PhotonView, nameof(SetPlayerData_RPC), RpcTarget.All,
            new object[] {nickName, actorNum ,Random.Range(0,8)});

    }


    void InitializeControls()
    {
        GameEvents.GameplayUIEvents.SubmitDecks.Register(OnSubmitDeck);
    }

    private void OnDisable()
    {
        GameEvents.NetworkEvents.NetworkDisconnectedEvent.UnRegister(OnNetworkDisconnect);
        GameEvents.GameplayUIEvents.SubmitDecks.UnRegister(OnSubmitDeck);
        GameEvents.GameplayEvents.RoundCompleted.UnRegister(OnPlayerSubmitScore);
    }

    protected virtual void OnSubmitDeck()
    {
        GameEvents.NetworkGameplayEvents.NetworkSubmitRequest.Raise( new NetworkDataObject(GameCardsData.Instance.GetDecksData(), ID)); 
    }
    private void OnPlayerSubmitScore()
    {
        GameEvents.GameplayEvents.OnPlayerScoreSubmit.Raise(ID,score);
    }

    public override void AwardPlayerPoints(int reward)
    {
        NetworkManager.NetworkUtilities.RaiseRPC(m_PhotonView, nameof(SyncAwardPlayerPoints_RPC), RpcTarget.All,
            new object[] { ID , reward});
    }
    
    

    [PunRPC]
    public virtual void SyncAwardPlayerPoints_RPC(int id, int reward)
    {
        if (id != ID)
            return;
        
        score += reward; 
        
        print($"Award Rewarded {ID} : {LocalID} : {reward}");
        
        GameEvents.GameplayEvents.OnPlayerScoreSubmit.Raise(ID,score);
        GameEvents.GameplayEvents.PlayerScoreReceived.Raise(score, LocalID);
        
        if(!IsLocalPlayer || IsBot)
            return;
        
        GameEvents.GameplayUIEvents.PlayerRewardReceived.Raise(reward);
    }
    
    public override void SubmitCardData(string data)
    {
        NetworkManager.NetworkUtilities.RaiseRPC(m_PhotonView, nameof(ReceiveHandFromNetwork), RpcTarget.All, 
            new object[] { data, ID });
    }

    [PunRPC]
    public virtual void ReceiveHandFromNetwork(string data, int _ID)
    {
        print($"Player Bot Hand {_ID}");
        if (!m_PhotonView.IsMine)
            return;

        GameEvents.NetworkEvents.PlayerReceiveCardsData.Raise(data, _ID);
    }

    [PunRPC]
    public void SetPlayerData_RPC(string nameToSet,int localID,int avatarID)
    {
        Name = nameToSet;
        LocalID = localID;
        CharacterAvatarID = avatarID;
        
        OnPlayerDataSet();
    }

    private void OnPlayerDataSet()
    {
        GameEvents.GameplayEvents.LocalPlayerJoined.Raise(new PlayerViewDataObject()
        {
            Name = Name,
            LocalID = LocalID,
            AvatarID = CharacterAvatarID
        });        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIViewListenerObject : PlayerViewListenerObject
{
    [SerializeField] private List<PlayerScoreUIObject> m_ScoreObjects = new();

    protected override void OnEnable()
    {
        base.OnEnable();
        GameEvents.GameplayEvents.PlayerScoreReceived.Register(OnPlayerScoreReceived);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEvents.GameplayEvents.PlayerScoreReceived.UnRegister(OnPlayerScoreReceived);
    }

    private void OnPlayerScoreReceived(int score, int playerId)
    {
        PlayerScoreUIObject scoreObject = m_ScoreObjects.Find(obj => obj.PositionIndex == playerId);
        scoreObject.SetScore(score);
    }
    
    protected override void OnLocalPlayerJoined(PlayerViewDataObject viewDataObject)
    {
        PlayerScoreUIObject scoreObject = m_ScoreObjects.Find(obj => obj.PositionIndex == viewDataObject.LocalID);
        scoreObject.SetContainerStatus(true);
        scoreObject.SetName(viewDataObject.Name);
        print( $"Name is {viewDataObject.Name}");

    }
}

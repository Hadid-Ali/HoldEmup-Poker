using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTableViewListenerObject : PlayerViewListenerObject
{
    [SerializeField] private List<PlayerTablePosition> m_TablePositions = new();
    [SerializeField] private GameObject m_CardsHeaderObject;

    protected override void OnEnable()
    {
        base.OnEnable();
        GameEvents.GameFlowEvents.MatchOver.Register(OnMatchOver);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEvents.GameFlowEvents.MatchOver.UnRegister(OnMatchOver);
    }

    private void OnMatchOver()
    {
        m_CardsHeaderObject.SetActive(false);
    }

    protected override void OnLocalPlayerJoined(PlayerViewDataObject viewDataObject)
    {
        PlayerTablePosition position =
            m_TablePositions.Find(player => player.TablePositionIndex == viewDataObject.LocalID);
        
        position.SetAvatarIndex(viewDataObject.AvatarID);
    }
}

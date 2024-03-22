using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTablePositionView : MonoBehaviour
{
    [SerializeField] private EnableObjectComponent m_CharacterAvatarContainer;

    public void SetPositionEnabled(bool status)
    {
        m_CharacterAvatarContainer.SetContainerActiveState(status);
    }

    public void SelectCharacterAtIndex(int index)
    {
        m_CharacterAvatarContainer.EnableAtIndex(index);
    }
}

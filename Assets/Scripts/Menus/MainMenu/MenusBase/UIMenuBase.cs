using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIMenuBase : MonoBehaviour
{
    [Header("Base Menu Refs")]
    
    [SerializeField] private MenuName m_MenuName;
    [SerializeField] private GameObject m_MenuContainer;

    public MenuName MenuName => m_MenuName;

    public void SetMenuActiveState(bool isActive)
    {
        m_MenuContainer.SetActive(isActive);

        if (isActive)
            OnContainerEnable();
        else
            OnContainerDisable();
    }

    protected virtual void OnContainerEnable()
    {
        
    }

    protected virtual void OnContainerDisable()
    {
        
    }

    public void ChangeMenuState(MenuName menuName)
    {
        GameEvents.MenuEvents.MenuTransition.Raise(menuName);
    }
}

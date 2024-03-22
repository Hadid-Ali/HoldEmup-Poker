using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public abstract class MenusController : MonoBehaviour
{
    [SerializeField] private List<UIMenuBase> m_Menus;
    
    private MenuName m_CurrentMenuStates;

    private void Start()
    {
        SetMenuState(MenuName.MainMenu);
    }

    protected virtual void OnEnable()
    {
        GameEvents.MenuEvents.MenuTransitionEvent.Register(OnMenuTransition);
    }

    protected virtual  void OnDisable()
    {
        GameEvents.MenuEvents.MenuTransitionEvent.UnRegister(OnMenuTransition);
    }
    
    protected void SetMenuState(MenuName menuName)
    {
        SetMenuState_Internal(menuName);
        HideAllMenus();

        if (menuName is MenuName.None)
            return;
        m_Menus.Find(x => x.MenuName == menuName).SetMenuActiveState(true);
    }
    
    private void OnMenuTransition(MenuName menuName)
    {
        SetMenuState(menuName);
    }

    private void SetMenuState_Internal(MenuName menuName)
    {
        m_CurrentMenuStates = menuName;
    }
    
    private void HideAllMenus()
    {
        for (int i = 0; i < m_Menus.Count; i++)
        {
            m_Menus[i].SetMenuActiveState(false);
        }
    }
}
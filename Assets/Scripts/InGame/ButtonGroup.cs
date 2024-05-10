using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    [SerializeField] private List<BetAction> actions;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private GameObject groupParent;

    private Action<BetAction> _onClick;

    private void Awake()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            int j = i;
            buttons[j].onClick.AddListener(() => OnClick(actions[j]));
        }
    }


    public void SubscribeButtonCalls(Action<BetAction> action)
    {
        _onClick += action;
    }

    public void EnableActionButton(BetAction action, bool val)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i] != action) continue;
            buttons[i].gameObject.SetActive(val);
            break;
        }
    }

    public void EnableAllButtons(bool val)
    {
        groupParent.SetActive(val);
    }

    private void OnClick(BetAction action)
    {
        _onClick?.Invoke(action);
        EnableAllButtons(false);
        print($"{action}");
    }


}

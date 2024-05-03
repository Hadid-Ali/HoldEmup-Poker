using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleBehaviorGroup : MonoBehaviour
{
    [SerializeField] private ToggleBehavior[] toggles;
    [SerializeField] private int selectedToggleIndex;
    [SerializeField] private BetAction[] actions;

    [SerializeField] private Slider raiseSlider;
    
    
    private void OnValidate()
    {
        toggles = GetComponentsInChildren<ToggleBehavior>();
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].SetIndex(i);
            toggles[i].gameObject.name = actions[i].ToString();
        }

        selectedToggleIndex = Mathf.Clamp(selectedToggleIndex,0,toggles.Length - 1);

        UpdateView();
    }

    private void Awake()
    {
        foreach (var t in toggles)
            t.Initialize(OnClick);
    }

    public void EnableActionButton(BetAction action, bool val)
    {
        int togIndex = -1;
        for (int i = 0; i < actions.Length; i++)
        {
            if (actions[i] != action) continue;
            togIndex = i;
            break;
        }
        if(togIndex == -1)
            return;
        
        toggles[togIndex].gameObject.SetActive(val);
    }

    private void OnClick(int index)
    {
        selectedToggleIndex = index;
        UpdateView();
    }

    public BetAction GetSelectedAction()
    {
        return actions[selectedToggleIndex];
    }

    private void UpdateView()
    {
        raiseSlider.gameObject.SetActive(GetSelectedAction() == BetAction.Raise);
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].isOn = i == selectedToggleIndex;
            toggles[i].OnViewUpdate(toggles[i].isOn);
        }
    }
}

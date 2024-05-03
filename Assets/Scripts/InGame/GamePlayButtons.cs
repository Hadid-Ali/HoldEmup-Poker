using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayButtons : MonoBehaviour
{
    [SerializeField] private ToggleBehaviorGroup group;
    [SerializeField] private Slider raiseSlider;
    [SerializeField] private Button button;

    public static Action<BetAction, int> OnPlayerActionSubmit;
    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        button.interactable = false;
        NetworkPlayer.OnEnableTurn += OnEnableTurn;
        NetworkPlayer.OnEnableAction += OnEnableAction;
    }
    private void OnDestroy()
    {
        NetworkPlayer.OnEnableTurn -= OnEnableTurn;
        NetworkPlayer.OnEnableAction -= OnEnableAction;
    }

    private void OnEnableTurn(bool obj)
    {
        button.interactable = obj;
    }

    private void OnEnableAction(BetAction action, bool val)
    {
        group.EnableActionButton(action, val);
    }
    
    private void OnButtonClick()
    {
        BetAction selectedAction = group.GetSelectedAction();
        int betAmount = selectedAction == BetAction.Raise ? (int) raiseSlider.value : 0;
        
        OnPlayerActionSubmit.Invoke(selectedAction, betAmount);
        button.interactable = false;
    }
}

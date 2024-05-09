using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GamePlayButtons : MonoBehaviour
{
    [SerializeField] private ToggleBehaviorGroup group;
    [SerializeField] private Slider raiseSlider;
    [SerializeField] private Button okButton;

    public static Action<BetAction, int> OnPlayerActionSubmit;
    private BetAction _lastAction;

    private void Awake()
    {
       group.SubscribeButtonCalls(OnButtonClick);
       okButton.onClick.AddListener(OnOkButtonClick);
    }

    private void Start()
    {
        NetworkPlayer.OnEnableTurn += OnEnableTurn;
        NetworkPlayer.OnEnableAction += OnEnableAction;
    }

    private void OnOkButtonClick()
    {
        raiseSlider.gameObject.SetActive(false);
        
        int betAmount = (int) raiseSlider.value ;
        BetAction selectedAction = group.GetSelectedAction();
        OnPlayerActionSubmit.Invoke(selectedAction, betAmount);
    }

    private void OnButtonClick(BetAction obj)
    {
        if (obj == BetAction.Raise)
        {
            raiseSlider.gameObject.SetActive(true);
            return;
        }
        
        BetAction selectedAction = group.GetSelectedAction();
        int betAmount = 0;
        OnPlayerActionSubmit.Invoke(selectedAction, betAmount);
    }
    

    private void OnDestroy()
    {
        NetworkPlayer.OnEnableTurn -= OnEnableTurn;
        NetworkPlayer.OnEnableAction -= OnEnableAction;
    }

    private void OnEnableTurn(bool obj)
    {
        group.EnableAllButtons(obj);
    }
    private void OnEnableAction(BetAction action, bool val)
    {
        group.EnableActionButton(action, val);
    }
}

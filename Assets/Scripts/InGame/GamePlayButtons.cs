using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GamePlayButtons : MonoBehaviour
{
    [SerializeField] private ButtonGroup group;
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
        raiseSlider.transform.parent.gameObject.SetActive(false);
        
        int betAmount = (int) raiseSlider.value ;
        OnPlayerActionSubmit.Invoke(_lastAction, betAmount);
    }

    private void OnButtonClick(BetAction obj)
    {
        _lastAction = obj;
        if (obj == BetAction.Raise)
        {
            raiseSlider.transform.parent.gameObject.SetActive(true);
            return;
        }
        int betAmount = 0;
        OnPlayerActionSubmit.Invoke(obj, betAmount);
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

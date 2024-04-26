using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayButtons : MonoBehaviour
{
    [SerializeField] private ToggleBehaviorGroup group;
    [SerializeField] private Button button;

    public static Action<BetAction> OnPlayerActionSubmit;
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
        OnPlayerActionSubmit.Invoke(group.GetSelectedAction());
        button.interactable = false;
    }
}

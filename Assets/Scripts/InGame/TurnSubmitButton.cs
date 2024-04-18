using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnSubmitButton : MonoBehaviour
{
    [SerializeField] private ToggleBehaviorGroup group;
    [SerializeField] private Button button;

    public static Action<BetAction> OnPlayerActionSubmit;
    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        button.interactable = false;
        NetworkPlayer.OnEnableTurn += OnEnableTurn;
    }

    private void OnEnableTurn(bool obj)
    {
        button.interactable = obj;
    }

    private void OnDestroy()
    {
        NetworkPlayer.OnEnableTurn -= OnEnableTurn;
    }

    private void OnButtonClick()
    {
        OnPlayerActionSubmit.Invoke(group.GetSelectedAction());
        button.interactable = false;
    }
}

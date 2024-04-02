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
        NetworkPlayer.OnEnableTurn += (b) => button.interactable = b;
    }

    private void OnDestroy()
    {
        NetworkPlayer.OnEnableTurn -= (b) => button.interactable = b;
    }

    private void OnButtonClick()
    {
        OnPlayerActionSubmit.Invoke(group.GetSelectedAction());
    }
}

using UnityEngine;

public class ToggleBehaviorGroup : MonoBehaviour
{
    [SerializeField] private ToggleBehavior[] toggles;
    [SerializeField] private int selectedToggleIndex;
    [SerializeField] private BetAction[] actions;
    
    private void OnValidate()
    {
        toggles = GetComponentsInChildren<ToggleBehavior>();
        for (int i = 0; i < toggles.Length; i++)
            toggles[i].SetIndex(i);

        selectedToggleIndex = Mathf.Clamp(selectedToggleIndex,0,toggles.Length - 1);

        UpdateView();
    }

    private void Awake()
    {
        foreach (var t in toggles)
            t.Initialize(OnClick);
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
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].isOn = i == selectedToggleIndex;
            toggles[i].OnViewUpdate(toggles[i].isOn);
        }
    }
}

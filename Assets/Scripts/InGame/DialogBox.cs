using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    [SerializeField] private GameObject levelWinDialog;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;
    [SerializeField] private CanvasGroup _group;

    private void ResetDialogBox()
    {
        levelWinDialog.SetActive(false);
        button.interactable = true;
        _group.alpha = 0;

        button.onClick.RemoveAllListeners();
    }

    public void Initialize(string _text, Action SomeFunction)
    {
        text.SetText(_text);
        
        button.onClick.AddListener(SomeFunction.Invoke);
        button.onClick.AddListener(OnButtonClick);
        
        
        levelWinDialog.SetActive(true);
        _group.DOFade(1, 1);
    }
    

    private void OnButtonClick()
    {
        ResetDialogBox();
    }
    
}

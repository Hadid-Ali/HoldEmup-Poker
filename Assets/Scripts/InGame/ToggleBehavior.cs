using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ToggleBehavior : MonoBehaviour
{
    [SerializeField] private Color normalColor; 
    [SerializeField] private Color selectedColor; 
    
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private int index; 
    public bool isOn;

    private Action<int> _executableFunc;

    public void SetIndex(int val) => index = val;

    private void OnValidate()
    {
        _button ??= GetComponent<Button>();
        _image ??= GetComponent<Image>();
        _text ??= GetComponentInChildren<TextMeshProUGUI>();
       // OnViewUpdate(isOn);
    }

    public void OnViewUpdate(bool val) => _image.color = val? selectedColor : normalColor;
    public void EnableClick(bool val) => _button.interactable = val;

    private void Onclick()
    {
        _executableFunc?.Invoke(index);
        isOn = true;
    }

    public void Initialize(Action<int> OnclickF)
    {
        _button.onClick.AddListener(Onclick);
        _executableFunc = OnclickF;
    }
}

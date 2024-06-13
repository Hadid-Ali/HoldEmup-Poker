using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaiseSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI minVal;
    [SerializeField] private TextMeshProUGUI maxVal;
    
    [SerializeField] private TextMeshProUGUI Val;
    private void Awake()
    {
        GameEvents.NetworkPlayerEvents.OnSetPlayerRaiseLimits.Register(UpdateSlider);
        slider.onValueChanged.AddListener(OnValueChanged);
    }
    private void OnDestroy() => GameEvents.NetworkPlayerEvents.OnSetPlayerRaiseLimits.UnRegister(UpdateSlider);

    private void OnEnable()
    {
        Val.SetText(slider.value.ToString());
    }
    private void OnValueChanged(float arg0)
    {
        int val = (int)arg0; 
        Val.SetText(val.ToString());
    } 
    private void UpdateSlider(int arg1, int arg2)
    {
        slider.minValue = arg1;
        slider.maxValue = arg2;
        
        minVal.SetText(arg1.ToString());
        maxVal.SetText(arg2.ToString());

        slider.value = 1;
        Val.SetText(slider.value.ToString());
        
        if (arg1 == arg2)
            slider.fillRect.GetComponent<Image>().fillAmount = 1;
    }

}

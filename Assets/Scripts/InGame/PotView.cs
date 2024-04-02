using TMPro;
using UnityEngine;


public class PotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    
    private void Start() => Pot.OnPotUpdate += OnPotViewUpdate;
    private void OnDestroy() => Pot.OnPotUpdate -= OnPotViewUpdate;

    private void OnPotViewUpdate(int obj)
    {
        textMeshProUGUI.SetText(obj.ToString());
        print("Add To pot text updated");
    } 
}

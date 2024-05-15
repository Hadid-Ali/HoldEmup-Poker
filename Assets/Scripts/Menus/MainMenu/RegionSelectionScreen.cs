using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegionSelectionScreen : UIMenuBase
{
    [SerializeField] private TMP_Dropdown m_RegionsListDropDown;
    [SerializeField] private TextMeshProUGUI m_BestRegionText;

    [SerializeField] private Button m_ConnectButton;

    private Region m_SelectedRegion;
    private List<Region> m_Regions = new();

    private void Start()
    {
        m_RegionsListDropDown.onValueChanged.AddListener(OnDropDownSelectionEvent);
        m_ConnectButton.onClick.AddListener(OnRegionSelected);
    }

    void OnPointerClick(PointerEventData eventData)
    {
        if (m_SelectedRegion == null)
            OnDropDownSelectionEvent(0);
    }

    private void OnEnable()
    {
        GameEvents.NetworkEvents.ConnectionTransition.Register(OnRegionsDataReceived);
    }

    private void OnDisable()
    {
        GameEvents.NetworkEvents.ConnectionTransition.UnRegister(OnRegionsDataReceived);
    }

    private void OnDropDownSelectionEvent(int index)
    {
        m_SelectedRegion = m_Regions[index];
        Debug.LogError($"Select Region {m_SelectedRegion}");
    }

    public void OnRegionsDataReceived(RegionConfig regionConfig)
    {
        SetupList(regionConfig);
        ChangeMenuState(MenuName.RegionSelectionScreen);
    }

    void SetupList(RegionConfig regionConfig)
    {
        m_RegionsListDropDown.ClearOptions();
        m_Regions = regionConfig.Availableregions;

        List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < m_Regions.Count; i++)
        {
            optionDataList.Add(new TMP_Dropdown.OptionData()
            {
                text = GetRegionString(m_Regions[i])
            });
        }

        m_RegionsListDropDown.options = optionDataList;
        m_BestRegionText.text = GetRegionString(regionConfig.BestRegion);
        OnDropDownSelectionEvent(0);
    }

    private string GetRegionString(Region region) =>
        $"{NetworkManager.Instance.RegionsRegistry.GetRegionName(region.Code.Trim()).Trim()}";

    public void OnRegionSelected()
    {
        if (m_SelectedRegion == null)
        {
            OnDropDownSelectionEvent(0);
        }

        GameEvents.NetworkEvents.PlayerRegionSelect.Raise(m_SelectedRegion);
        ChangeMenuState(MenuName.ConnectionScreen);
    }
}

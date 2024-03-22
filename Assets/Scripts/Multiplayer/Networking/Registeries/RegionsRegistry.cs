using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RegionMapObject
{
    public string RegionCode;
    public string RegionName;
}

[CreateAssetMenu(fileName = "Regions Registry", menuName = "Networking/Registries/Network Region")]
public class RegionsRegistry : ScriptableObject
{
    [SerializeField] private List<RegionMapObject> m_RegionsMap;

    public string GetRegionName(string regionCode) =>
        m_RegionsMap.Find(region => region.RegionCode == regionCode).RegionName ?? "Unnamed";
}

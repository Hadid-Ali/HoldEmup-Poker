using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityAtoms;
using UnityEngine;

public struct RegionConfig
{
    public List<Region> Availableregions;
    public Region BestRegion;
}

[EditorIcon("atom-icon-cherry")]
[CreateAssetMenu(menuName = "Unity Atoms/Events/Custom/Networking/ConnectionTransitionEvent", fileName = "ConnectionEvent")]
public sealed class ConnectionTransitionEvent : AtomEvent<RegionConfig>
{
    
}

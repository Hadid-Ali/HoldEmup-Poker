using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityAtoms;
using UnityEngine;


[EditorIcon("atom-icon-cherry")]
[CreateAssetMenu(menuName = "Unity Atoms/Events/Custom/Networking/PhotonRoomEvent", fileName = "PhotonRoomEvent")]
public sealed class PhotonRoomCreationEvent: AtomEvent<RoomOptions>
{
    
}

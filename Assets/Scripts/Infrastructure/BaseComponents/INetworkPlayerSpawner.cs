using System;

public interface INetworkPlayerSpawner
{
    public void Initialize(Action<NetworkPlayer> onPlayerSpawned);
    public void SpawnPlayer();
    public void RegisterPlayer(NetworkPlayer playerController);
    public void ReIteratePlayerSpawns();
    public NetworkPlayer GetPlayerAgainstID(int ID);
    public string GetPlayerName(int ID);
    public int GetLocalPlayerNetworkID();
    public int GetPlayerLocalID(int ID);
}

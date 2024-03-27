

[System.Serializable]
public struct PlayerData 
{
    public string NickName => _nickName;
    private string _nickName;

    public uint Stack => _stack;
    private uint _stack;

    public PlayerData(string nickName, uint stack)
    {
        _nickName = nickName;
        _stack = stack;
    }

    public void SetDefaultValues()
    {
        _nickName = "Player";
        _stack = 100;
    }
    
}
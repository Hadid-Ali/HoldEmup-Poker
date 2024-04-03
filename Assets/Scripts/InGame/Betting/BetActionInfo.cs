using Photon.Realtime;

public struct BetActionInfo
{
    public readonly NetworkPlayer Player;
    public readonly BetAction BetAction;
    public readonly uint BetAmount;

    public BetActionInfo(NetworkPlayer player, BetAction betAction, uint betAmount)
    {
        Player = player;
        BetAction = betAction;
        BetAmount = betAmount;
    }
}

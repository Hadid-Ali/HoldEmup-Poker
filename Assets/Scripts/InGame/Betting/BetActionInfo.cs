using Photon.Realtime;

public struct BetActionInfo
{
    public readonly NetworkPlayer Player;
    public readonly BetAction BetAction;
    public readonly int BetAmount;

    public BetActionInfo(NetworkPlayer player, BetAction betAction, int betAmount)
    {
        Player = player;
        BetAction = betAction;
        BetAmount = betAmount;
    }
}

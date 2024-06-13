public class ActionReport 
{
    public string PlayerID = "";
    public string MatchID = "";
    
    public string BetAction = "";
    public int BetAmount = 0;

    public override string ToString()
    {
        return $"Player ID : {PlayerID}, " +
               $"Match ID : {MatchID}," +
               $"BetAction ID : {BetAction}," +
               $"BetAmount : {BetAmount},";
    }
}

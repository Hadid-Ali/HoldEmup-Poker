public class ActionReport 
{
    public string PlayerID = "";
    public string MatchID = "";
    
    public string BetAction = "";
    public int BetAmount = 0;

    public override string ToString()
    {
        return $"Player ID : {PlayerID} \n " +
               $"Match ID : {MatchID} \n" +
               $"BetAction ID : {BetAction} \n" +
               $"BetAmount : {BetAmount} \n";
    }
}

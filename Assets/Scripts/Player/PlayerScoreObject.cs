using System.Collections.Generic;

[System.Serializable]
public class PlayerScoreObject
{
    public int UserID;
    public int Score;
    
    public List<int> WinningHandsIndexes;

    public PlayerScoreObject(int userID)
    {
        UserID = userID;
        Score = 0;
        WinningHandsIndexes = new();
    }

    public void AddScore(int score, int handIndex)
    {
        Score += score;
        WinningHandsIndexes.Add(handIndex);
    }
}

using UnityEngine;

public delegate void OnPlayerActionReport(ActionReport actionReport);
public class TrackableEvents : MonoBehaviour
{
    public static OnPlayerActionReport OnActionReport;
    
    private void Awake() => GameEvents.NetworkEvents.OnPlayerBetAction.Register(OnPlayerAction);

    private void OnDestroy() => GameEvents.NetworkEvents.OnPlayerBetAction.Register(OnPlayerAction);

    private void OnPlayerAction(ActionReport obj)
    {
        OnActionReport?.Invoke(obj);
        string yo = JsonUtility.ToJson(obj);
        
        print($"Action Report : \n {obj.PlayerID} \n {obj.MatchID} \n {obj.BetAction} \n {obj.BetAmount}");
    }
}

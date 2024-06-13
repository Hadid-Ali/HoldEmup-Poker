using UnityEngine;

public delegate void OnPlayerActionReport(ActionReport actionReport);
public delegate void OnPlayerActionReportAPICallBack(string actionReport);
public class TrackableEvents : MonoBehaviour
{
    public static OnPlayerActionReport OnActionReport;
    public static OnPlayerActionReportAPICallBack OnActionReportAPICallBack;
    
    private void Awake() => GameEvents.NetworkEvents.OnPlayerBetAction.Register(OnPlayerAction);

    private void OnDestroy() => GameEvents.NetworkEvents.OnPlayerBetAction.Register(OnPlayerAction);

    private void OnPlayerAction(ActionReport obj)
    {
        string yo = JsonUtility.ToJson(obj);
        OnActionReport?.Invoke(obj);
        OnActionReportAPICallBack?.Invoke(yo);
        
        print($"Action Report : \n {obj.PlayerID} \n {obj.MatchID} \n {obj.BetAction} \n {obj.BetAmount}");
    }
}

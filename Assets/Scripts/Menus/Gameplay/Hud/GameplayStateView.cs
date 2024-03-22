using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class GameplayStateView : MonoBehaviour
{
    [Header("Components")] 
    
    [SerializeField] private GameplayHud m_Hud;
    [SerializeField] private CanvasGroup m_GameCardsContainer;

    [SerializeField] private GameObject m_CasinoViewObject;
    [SerializeField] private ResultUIView m_ResultUIView;

    [SerializeField] private GameplayHud m_GameplayHud;
    
    [Header("Properties")]
    
    [SerializeField] private float m_WaitForCardsToShow = 2f;
    
    private WaitForSeconds m_CardsContainerWait;
    
    private void Start()
    {
        m_CardsContainerWait  = new WaitForSeconds(m_WaitForCardsToShow);
    }

    private void OnEnable()
    {
        GameEvents.GameplayEvents.GameplayStateSwitched.Register(OnGameplayStateSwitched);
    }
    
    private void OnDisable()
    {
        GameEvents.GameplayEvents.GameplayStateSwitched.UnRegister(OnGameplayStateSwitched);
    }

    private void OnGameplayStateSwitched(GameplayState gameplayState)
    {
        switch (gameplayState)
        {
            case GameplayState.Casino_View:
                SwitchToCasinoView();
                break;

            case GameplayState.Cards_View:
                StartCoroutine(SwitchToCardsView());
                break;
            
            case GameplayState.Result_Deck_View:
                SwitchToResultantView();
                break;
            
            case GameplayState.Result_Score_View:
                SwitchToResultScoresView();
                break;
        }
    }

    private void SwitchToCasinoView()
    {
        Debug.LogError("Switch To Casino View");
        SetGameplayCardsViewState(false);
        SetResultView(false);
        SetCasinoViewObjectState(true);
    }

    private IEnumerator SwitchToCardsView()
    {
        SetResultView(false);
        m_CasinoViewObject.SetActive(false);
        m_Hud.ShowScoreOnUI();

        yield return m_CardsContainerWait;

        SetGameplayCardsViewState(true);
        SetCasinoViewObjectState(false);
    }

    private void SwitchToResultantView()
    {
        SetResultView(true);
        SetGameplayCardsViewState(false);
        SetCasinoViewObjectState(false);
    }

    private void SwitchToResultScoresView()
    {
        SetResultView(false);
        m_Hud.ShowResultMenu();
    }
    
    private void SetResultView(bool status)
    {
        m_ResultUIView.SetActiveState(status);
    }
    
    private void SetGameplayCardsViewState(bool state)
    {
        m_GameCardsContainer.alpha = state ? 1 : 0;
        m_GameCardsContainer.interactable = state;
        GameEvents.GameplayEvents.GameplayCardsStateChanged.Raise(state);
    }

    
    private void SetCasinoViewObjectState(bool state)
    {
        m_CasinoViewObject.SetActive(true);
     //   m_CasinoViewObject.SetViewState(state);
    }
}

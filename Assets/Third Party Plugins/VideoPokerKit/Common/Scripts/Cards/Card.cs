using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Card : MonoBehaviour 
{
	[SerializeField] protected Image m_CardFaceImage;
	[SerializeField] private Transform m_CardTransform;

	[SerializeField] private AudioSource flipSound;
	[SerializeField] private CardDataHolder m_CardDataHandler;

	private GameEvent m_CardDataUpdated = new();
	
	public CardData CardData => m_CardDataHandler.CardData;
	
	protected virtual void Start () 
	{
		Initialize();
	}

	void Initialize()
	{
		m_CardTransform = transform;
		flipSound = GetComponent<AudioSource>();

		m_CardDataHandler.Initialize(ClearAfterDeal, OnCardDataUpdatedByPlayer, SetCardDataView);
	}

	public void SetCardDataHolder(CardDataHolder card)
	{
		m_CardDataHandler = card;
	}
	
	public void InitializeWithAction(Action onCardDataUpdated)
	{
		m_CardDataUpdated.Register(onCardDataUpdated);
	}

	public void RefreshCard()
	{
		m_CardDataHandler.RefreshData();
	}

	public void SaveData()
	{
		m_CardDataHandler.SaveData();
	}

	public void SetData(CardData cardData,bool isPersistent,bool isInit)
	{
		m_CardDataHandler.SetCardData(cardData, isPersistent);

		if (isPersistent)
		{
			StartFlippingCard();
		}

	}

	private void OnCardDataUpdatedByPlayer()
	{
		m_CardDataUpdated.Raise();
	}
	
	public void SetActiveStatus(bool status)
	{
		m_CardFaceImage.enabled = status;
		StartFlippingCard();
	}

	public void SetCardDataView(CardData cardData)
	{
		UpdateCardFaceSprite(CardsRegistery.Instance.GetCardSprite(cardData.type, cardData.value));
	}
	
	public void StartFlippingCard()
	{		
		SetCardRotation(0);
		flipSound.Play();
	}

	public void UpdateCardFaceSprite(Sprite sprite)
	{
		m_CardFaceImage.sprite = sprite;
	}

	public void SetResultsState(bool bWin)
	{
		// if it's a winner card, we start a specific animation for the card (flashing)
		// otherwise we play a 'not winner' fade-out animation

		// if(bWin)
		// 	animator.SetTrigger ("trResultsWin");
		// else
		// 	animator.SetTrigger ("trResultsIgnore");
	}

	//----------------------------------------------------

	public void DealWithDelay(float delay)
	{
		Deal (); // start dealing in an instant
	}


	//----------------------------------------------------

	public void Deal()
	{
		StartFlippingCard();
		SoundsManager.the.dealCardSound.Play();
		Invoke(nameof(OnCardDealt), 1f);
	}

	//--------------------------------------------------

	public void ResetHold()
	{
		// disable HOLD
	//	m_CardData.hold = false;
		// hide hold marker
		Debug.LogError("Reset Hold");
	}

	public void ClearAfterDeal()
	{
		// // we vanish only from some states (not from dealing)
		// AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo (0);
		// if (stateInfo.IsName ("CardIdle") || 
		//     stateInfo.IsName ("CardHold") || 
		//     stateInfo.IsName ("CardResultsIgnore") || stateInfo.IsName ("CardResultsWin"))
		// {
		// 	animator.SetTrigger ("trVanish");
		// }
		Debug.LogError("Clear After Deal");
	}

	//--------------------------------------------------

	public void	VanishAndDealAgainWithDelay(float delay)
	{
		ClearAfterDeal();
		DealWithDelay(delay);
	}

	private void SetCardRotation(float rotation)
	{
		m_CardTransform.rotation = Quaternion.Euler(new Vector3(0f, rotation, 0f));
	}
	
	//--------------------------------------------------

	void OnMouseDown()
	{
		// if we click the card (each card has a collider attached), toggle HOLD
		if(MainGame.the.gameState == MainGame.STATE_WAIT_HOLD)
			ToggleHold();
	}

	//--------------------------------------------------

	public void ToggleHold()
	{
		// // change hold status
		// m_CardData.hold = !m_CardData.hold;
		//
		// // play HOLD animation if needed
		// if(m_CardData.hold)
		// {
		// 	// start playing the HOLD animation
		// 	Debug.LogError("Hold");
		// 	// play HOLD sound
		// 	SoundsManager.the.holdSound.Play ();
		// }
	}

	//--------------------------------------------------

	// this callback is called from the DEAL animation (as an Animation event)
	void OnCardDealt()
	{
		MainGame.the.CardAnimationFinished();
	}

	//--------------------------------------------------
}

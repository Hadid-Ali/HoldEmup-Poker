using System;
using UnityEngine;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour
{
    [SerializeField] private Image m_CardImge;
    [SerializeField] private Transform m_Transform;

    private Sprite m_DefaultSprite;

    protected void Start()
    {
        m_DefaultSprite = m_CardImge.sprite;
    }

    private void Update()
    {
        SetPositionToMouse();
        CheckForMouseUp();
    }
    
    public void EnableWithData(CardData cardData)
    {
        //Reset();
        m_CardImge.sprite = CardsRegistery.Instance.GetCardSprite(cardData.type, cardData.value);
        SetEnabled(true);
    }
    public void SetEnabled(bool enableStatus)
    {
        SetPositionToMouse();
        gameObject.SetActive(enableStatus);

        if (!enableStatus)
            Reset();
    }
    
    private void SetPositionToMouse()
    {
        m_Transform.position = Input.mousePosition;
    }

    private void Reset()
    {
        m_CardImge.sprite = null;
    }

    private void CheckForMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameEvents.GameplayEvents.CardDropEvent.Raise();
        }
    }
}

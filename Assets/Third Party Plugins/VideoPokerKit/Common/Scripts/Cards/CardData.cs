using System;
using UnityEngine;
using System.Collections;

//--------------------------------------------

// card suits
public enum CardType
{
	TYPE_HEARTS,
	TYPE_SPADES,
	TYPE_DIAMONDS,
	TYPE_CLUBS,
	TYPES_NO
}

//--------------------------------------------

// card values
public enum CardValue
{
	VALUE_2 = 0,
	VALUE_3 = 1,
	VALUE_4 = 2,
	VALUE_5 = 3,
	VALUE_6 = 4,
	VALUE_7 = 5,
	VALUE_8 = 6,
	VALUE_9 = 7,
	VALUE_10 = 8,
	VALUE_J = 9,
	VALUE_Q = 10,
	VALUE_K = 11,
	VALUE_A = 12,
	VALUES_NO = 13,
}

//--------------------------------------------

[System.Serializable]
public class CardData 
{
	// card suit and value (set in the Inspector)
	public CardType type = CardType.TYPE_HEARTS;
	public CardValue value = CardValue.VALUE_2;
	
	// if was dealt or not from the deck (used only for library cards)
	private bool m_dealt = false;

	public bool dealt
	{
		get => m_dealt;
		set => m_dealt = value;
	}

	//----------------------------------

	// used to make a copy of another card
	public void CopyInfoFrom(CardData other)
	{
		type = other.type;
		value = other.value;
	}

	public override string ToString()
	{
		return $"{value} of {type}";
	}
}

//--------------------------------------------
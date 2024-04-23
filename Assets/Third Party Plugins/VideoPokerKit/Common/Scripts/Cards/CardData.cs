using System;

public enum CardType
{
	TYPE_HEARTS,
	TYPE_SPADES,
	TYPE_DIAMONDS,
	TYPE_CLUBS,
	TYPES_NO
}
public enum CardValue
{
	value_2 = 0,
	value_3 = 1,
	value_4 = 2,
	value_5 = 3,
	value_6 = 4,
	value_7 = 5,
	value_8 = 6,
	value_9 = 7,
	value_10 = 8,
	value_J = 9,
	value_Q = 10,
	value_K = 11,
	value_A = 12,
	valueS_NO = 13,
}


[System.Serializable]
public class CardData
{
	// card suit and value (set in the Inspector)
	public CardType type = CardType.TYPE_HEARTS;
	public CardValue value = CardValue.value_2;

	private bool m_dealt = false;

	public bool dealt
	{
		get => m_dealt;
		set => m_dealt = value;
	}


	public int[] ConvertToIntArray()
	{
		int[] binaryData = new int[2];
		binaryData[0] = (int)type;
		binaryData[1] = (int)value;

		return binaryData;
	}

	public static CardData ConvertIntArrayToCardData(int[] binaryData)
	{
		CardData cardData = new CardData()
		{
			type = (CardType)binaryData[0],
			value = (CardValue)binaryData[1]
		};

		return cardData;
	}

	public void CopyInfoFrom(CardData other)
	{
		type = other.type;
		value = other.value;
	}
	
	public CardData()
	{

	}

	public CardData(CardType type, CardValue value)
	{
		this.type = type;
		this.value = value;
	}



	public static string RankToString(int rank)
	{
		return ((CardValue)rank).ToString();
	}

	public int GetRank()
	{
		return (int)value;
	}


	#region Overrides

	public override string ToString()
	{
		return $"{value} of {type}";
	}

	public static bool operator <(CardData a, CardData b)
	{
		return a.value < b.value;
	}

	public static bool operator >(CardData a, CardData b)
	{
		return a.value > b.value;
	}

	public static bool operator <=(CardData a, CardData b)
	{
		return a.value <= b.value;
	}

	public static bool operator >=(CardData a, CardData b)
	{
		return a.value >= b.value;
	}

	protected bool Equals(CardData other)
	{
		return type == other.type && value == other.value;
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj))
		{
			return false;
		}

		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		if (obj.GetType() != GetType())
		{
			return false;
		}

		return Equals((CardData)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine((int)type, (int)value);
	}
	
#endregion

}



	



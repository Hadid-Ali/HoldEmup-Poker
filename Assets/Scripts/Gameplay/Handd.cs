using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Source https://github.com/ccqi/TexasHoldem
/// </summary>
/// 
[Serializable]
public class Handd
{
   [SerializeField]public List<CardData> _Handd;
    private readonly List<int> _combinationId;

    public Handd(IEnumerable<CardData> cards)
    {
        _Handd = cards.ToList();
        _combinationId = new List<int>();
    }
    
    public Handd()
    {
        _Handd = new List<CardData>();
        _combinationId = new List<int>();
    }
    
    public Handd(Handd otherHandd)
    {
        _Handd = new List<CardData>(otherHandd._Handd);
        _combinationId = new List<int>();
    }
    
    public CardData this[int index] => _Handd[index];

    public List<CardData> GetMainCombination()
    {
        return _combinationId[0] switch
        {
            1 => new List<CardData> {_Handd[0]},
            2 => new List<CardData> {_Handd[0], _Handd[1]},
            3 => new List<CardData> {_Handd[0], _Handd[1], _Handd[2], _Handd[3]},
            4 => new List<CardData> {_Handd[0], _Handd[1], _Handd[2]},
            5 => new List<CardData> {_Handd[0], _Handd[1], _Handd[2], _Handd[3], _Handd[4]},
            6 => new List<CardData> {_Handd[0], _Handd[1], _Handd[2], _Handd[3], _Handd[4]},
            7 => new List<CardData> {_Handd[0], _Handd[1], _Handd[2], _Handd[3], _Handd[4]},
            8 => new List<CardData> {_Handd[0], _Handd[1], _Handd[2], _Handd[3]},
            9 => new List<CardData> {_Handd[0], _Handd[1], _Handd[2], _Handd[3], _Handd[4]},
            _ => new List<CardData> {_Handd[0], _Handd[1], _Handd[2], _Handd[3], _Handd[4]},
        };
    }
    
    public CardData GetCard(int index)
    {
        if (index >= _Handd.Count)
            throw new ArgumentOutOfRangeException();
        return _Handd[index];
    }

    public void Clear()
    {
        _Handd.Clear();
        _combinationId.Clear();
    }
    
    public void Add(CardData card)
    {
        _Handd.Add(card);
    }
    
    public void RemoveAt(int index)
    {
        _Handd.RemoveAt(index);
    }
    
    public void Remove(CardData card)
    {
        _Handd.Remove(card);
    }
    
    public void AddCombinationId(int value)
    {
        _combinationId.Add(value);
    }
    
    public int Count()
    {
        return _Handd.Count;
    }

    public void SortByRank()
    {
        _Handd = QuickSortRank(_Handd);
    }
    
    private List<CardData> QuickSortRank(List<CardData> myCards)
    {
        Random ran = new();

        if (myCards.Count() <= 1)
        {
            return myCards;
        }
        
        CardData pivot = myCards[ran.Next(myCards.Count())];
        myCards.Remove(pivot);

        List<CardData> less = new();
        List<CardData> greater = new();
        // Assign values to less or greater list
        foreach (CardData i in myCards)
        {
            if (i > pivot)
            {
                greater.Add(i);
            }
            else if (i <= pivot)
            {
                less.Add(i);
            }
        }
        // Recurse for less and greaterlists
        List<CardData> list = new();
        list.AddRange(QuickSortRank(greater));
        list.Add(pivot);
        list.AddRange(QuickSortRank(less));
        return list;
    }

    // ReSharper disable once UnusedMember.Local
    private List<CardData> QuickSortSuit(List<CardData> myCards)
    {
        Random ran = new();

        if (myCards.Count() <= 1)
        {
            return myCards;
        }
        
        CardData pivot = myCards[ran.Next(myCards.Count())];
        myCards.Remove(pivot);

        List<CardData> less = new();
        List<CardData> greater = new();
        // Assign values to less or greater list
        for (var i = 0; i < myCards.Count(); i++)
        {
            if (myCards[i].type > pivot.type)
            {
                greater.Add(myCards[i]);
            }
            else if (myCards[i].type <= pivot.type)
            {
                less.Add(myCards[i]);
            }
        }
        
        // Recurse for less and greaterlists
        List<CardData> list = new();
        list.AddRange(QuickSortSuit(less));
        list.Add(pivot);
        list.AddRange(QuickSortSuit(greater));
        return list;
    }

    #region Overrides
    public override string ToString()
    {
        if (_combinationId.Any() == false)
        {
            return "No Poker Handd is Found";
        }

        return _combinationId[0] switch
        {
            1 => CardData.RankToString(_combinationId[1]) + " High. Others: " + string.Join(", ", _combinationId.Skip(2)),
            2 => "One Pair of " + CardData.RankToString(_combinationId[1]) + "`s. Others: " + string.Join(", ", _combinationId.Skip(2)),
            3 => "Two Pairs: " + CardData.RankToString(_combinationId[1]) + "s over " + CardData.RankToString(_combinationId[2]) + "s. Kicker: " + CardData.RankToString(_combinationId[3]),
            4 => "Three " + CardData.RankToString(_combinationId[1]) + "`s. Others: " + string.Join(", ", _combinationId.Skip(2)),
            5 => CardData.RankToString(_combinationId[1]) + " High Straight",
            6 => CardData.RankToString(_combinationId[1]) + " High Flush. Kicker: " + _combinationId[2],
            7 => CardData.RankToString(_combinationId[1]) + "`s Full of " + CardData.RankToString(_combinationId[2]) + "s",
            8 => "Quad " + CardData.RankToString(_combinationId[1]) + "s Kicker: " + _combinationId[2],
            9 => CardData.RankToString(_combinationId[1]) + " High Straight Flush",
            _ => "Royal Flush"
        };
    }
    public static bool operator ==(Handd a, Handd b)
    {
        for (var i = 0; i < a!._combinationId.Count(); i++)
        {
            if (a._combinationId[i] != b!._combinationId[i])
            {
                return false;
            }
        }
        return true;
    }
    public static bool operator !=(Handd a, Handd b)
    {
        if (a == null | b == null)
        {
            throw new NullReferenceException();
        }
        
        if (a._combinationId.Count == 0 || b._combinationId.Count == 0)
            throw new NullReferenceException();
        for (var i = 0; i < a._combinationId.Count(); i++)
        {
            if (a._combinationId[i] != b._combinationId[i])
            {
                return true;
            }
        }
        return false;
    }
    public static bool operator <(Handd a, Handd b)
    {
        if (a._combinationId.Count == 0 || b._combinationId.Count == 0)
            throw new NullReferenceException();
        for (var i = 0; i < a._combinationId.Count(); i++)
        {
            if (a._combinationId[i] < b._combinationId[i])
            {
                return true;
            }
            if (a._combinationId[i] > b._combinationId[i])
            {
                return false;
            }
        }
        return false;
    }
    public static bool operator >(Handd a, Handd b)
    {
        if (a._combinationId.Count == 0 || b._combinationId.Count == 0)
            throw new NullReferenceException();
        for (var i = 0; i < a._combinationId.Count; i++)
        {
            if (a._combinationId[i] > b._combinationId[i])
            {
                return true;
            }
            if (a._combinationId[i] < b._combinationId[i])
            {
                return false;
            }

        }
        return false;
    }
    public static bool operator <=(Handd a, Handd b)
    {
        if (a._combinationId.Count == 0 || b._combinationId.Count == 0)
            throw new NullReferenceException();
        for (var i = 0; i < a._combinationId.Count; i++)
        {
            if (a._combinationId[i] < b._combinationId[i])
            {
                return true;
            }
            if (a._combinationId[i] > b._combinationId[i])
            {
                return false;
            }

        }
        return true;
    }
    public static bool operator >=(Handd a, Handd b)
    {
        if (a._combinationId.Count == 0 || b._combinationId.Count == 0)
            throw new NullReferenceException();
        for (var i = 0; i < a._combinationId.Count(); i++)
        {
            if (a._combinationId[i] > b._combinationId[i])
            {
                return true;
            }
            if (a._combinationId[i] < b._combinationId[i])
            {
                return false;
            }

        }
        return true;
    }
    public static Handd operator +(Handd a, Handd b)
    {
        for (var i = 0; i < b.Count(); i++)
        {
            a.Add(b[i]);
        }
        return a;
    } 
    private bool Equals(Handd other)
    {
        return Equals(_Handd, other._Handd) && Equals(_combinationId, other._combinationId);
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

        return Equals((Handd)obj);
    }
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return HashCode.Combine(_Handd, _combinationId);
    }
    #endregion
}
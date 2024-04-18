
using System.Collections.Generic;
using UnityEngine;

public class CardsRegistery : MonobehaviourSingleton<CardsRegistery>
{
    [SerializeField] private List<CardDataObject> m_Cards = new();

    public Sprite GetCardSprite(CardType cardType, CardValue cardValue) =>
        m_Cards.Find(card => card.type == cardType && card.value == cardValue).CardImage;
    

    //To Automate Card Fetching
     // public List<Sprite> Clubs = new ();
     // public List<Sprite> Diamonds = new ();
     // public List<Sprite> Hearts = new ();
     // public List<Sprite> Spades = new ();
     //
     // [ContextMenu("Put Images")]
     // private void ManageCards()
     // {
     //
     //     var prefabInstance = Selection.activeGameObject;
     //     if (prefabInstance == null) return;
     //     // Modify properties of the prefab instance here...
     //     foreach (var v in m_Cards)
     //     {
     //
     //         v.CardImage = v.type switch
     //         {
     //             CardType.TYPE_CLUBS => (int)v.value switch
     //             {
     //                 0 => Clubs.Find(x => x.name == "2"),
     //                 1 => Clubs.Find(x => x.name == "3"),
     //                 2 => Clubs.Find(x => x.name == "4"),
     //                 3 => Clubs.Find(x => x.name == "5"),
     //                 4 => Clubs.Find(x => x.name == "6"),
     //                 5 => Clubs.Find(x => x.name == "7"),
     //                 6 => Clubs.Find(x => x.name == "8"),
     //                 7 => Clubs.Find(x => x.name == "9"),
     //                 8 => Clubs.Find(x => x.name == "10"),
     //                 9 => Clubs.Find(x => x.name == "J"),
     //                 10 => Clubs.Find(x => x.name == "Q"),
     //                 11 => Clubs.Find(x => x.name == "K"),
     //                 12 => Clubs.Find(x => x.name == "A"),
     //                 13 => Clubs.Find(x => x.name == "BACK"),
     //                 _ => v.CardImage
     //
     //             },
     //             CardType.TYPE_HEARTS => (int)v.value switch
     //             {
     //                 0 => Hearts.Find(x => x.name == "2"),
     //                 1 => Hearts.Find(x => x.name == "3"),
     //                 2 => Hearts.Find(x => x.name == "4"),
     //                 3 => Hearts.Find(x => x.name == "5"),
     //                 4 => Hearts.Find(x => x.name == "6"),
     //                 5 => Hearts.Find(x => x.name == "7"),
     //                 6 => Hearts.Find(x => x.name == "8"),
     //                 7 => Hearts.Find(x => x.name == "9"),
     //                 8 => Hearts.Find(x => x.name == "10"),
     //                 9 => Hearts.Find(x => x.name == "J"),
     //                 10 => Hearts.Find(x => x.name == "Q"),
     //                 11 => Hearts.Find(x => x.name == "K"),
     //                 12 => Hearts.Find(x => x.name == "A"),
     //                 13 => Hearts.Find(x => x.name == "BACK"),
     //                 _ => v.CardImage
     //             },
     //             CardType.TYPE_DIAMONDS => (int)v.value switch
     //             {
     //                 0 => Diamonds.Find(x => x.name == "2"),
     //                 1 => Diamonds.Find(x => x.name == "3"),
     //                 2 => Diamonds.Find(x => x.name == "4"),
     //                 3 => Diamonds.Find(x => x.name == "5"),
     //                 4 => Diamonds.Find(x => x.name == "6"),
     //                 5 => Diamonds.Find(x => x.name == "7"),
     //                 6 => Diamonds.Find(x => x.name == "8"),
     //                 7 => Diamonds.Find(x => x.name == "9"),
     //                 8 => Diamonds.Find(x => x.name == "10"),
     //                 9 => Diamonds.Find(x => x.name == "J"),
     //                 10 => Diamonds.Find(x => x.name == "Q"),
     //                 11 => Diamonds.Find(x => x.name == "K"),
     //                 12 => Diamonds.Find(x => x.name == "A"),
     //                 13 => Diamonds.Find(x => x.name == "BACK"),
     //                 _ => v.CardImage
     //             },
     //             CardType.TYPE_SPADES => (int)v.value switch
     //             {
     //                 0 => Spades.Find(x => x.name == "2"),
     //                 1 => Spades.Find(x => x.name == "3"),
     //                 2 => Spades.Find(x => x.name == "4"),
     //                 3 => Spades.Find(x => x.name == "5"),
     //                 4 => Spades.Find(x => x.name == "6"),
     //                 5 => Spades.Find(x => x.name == "7"),
     //                 6 => Spades.Find(x => x.name == "8"),
     //                 7 => Spades.Find(x => x.name == "9"),
     //                 8 => Spades.Find(x => x.name == "10"),
     //                 9 => Spades.Find(x => x.name == "J"),
     //                 10 => Spades.Find(x => x.name == "Q"),
     //                 11 => Spades.Find(x => x.name == "K"),
     //                 12 => Spades.Find(x => x.name == "A"),
     //                 13 => Spades.Find(x => x.name == "BACK"),
     //                 _ => v.CardImage
     //             },
     //             _ => v.CardImage
     //         };
     //         // Mark the prefab as dirty
     //            
     //     }
     //     PrefabUtility.RecordPrefabInstancePropertyModifications(prefabInstance);
     //     EditorUtility.SetDirty(gameObject);
     //     print("Marked Dirty");
     // }
}   

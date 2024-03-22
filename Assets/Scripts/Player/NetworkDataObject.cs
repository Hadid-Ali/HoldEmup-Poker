using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class NetworkDataObject
{
    public List<CardData> PlayerDecks = new();
    public int PhotonViewID;

    public NetworkDataObject(List<CardData> decks, int photonViewID)
    {
        PlayerDecks = decks;
        PhotonViewID = photonViewID;
    }

    public List<CardData[]> GetDeck()
    {
        List<CardData[]> decks = new();

        int handSize = GameData.MetaData.DeckSize;
        int handCount = GameData.MetaData.DecksCount;

        int cardIndex = 0;
        
        for (int i = 0; i < handCount; i++)
        {
            decks.Add(new CardData[handSize]);
            
            for (int j = 0; j < handSize; j++)
            {
                decks[i][j] = PlayerDecks[cardIndex++];
            }
        }

        return decks;
    }
    
    public static string Serialize(NetworkDataObject networkDataObject)
    {
        string data = JsonUtility.ToJson(networkDataObject);
        return data;
    }

    public static NetworkDataObject DeSerialize(string dataString)
    {
        return JsonUtility.FromJson<NetworkDataObject>(dataString);
    }
}

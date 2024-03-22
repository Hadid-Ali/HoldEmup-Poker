using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDeck : MonoBehaviour
{
    [SerializeField] private Image[] m_CardContainers;

    public void SetHandData(Sprite[] cardImages)
    {
        for (int i = 0; i < cardImages.Length; i++)
        {
            m_CardContainers[i].sprite = cardImages[i];
        }
    }
}

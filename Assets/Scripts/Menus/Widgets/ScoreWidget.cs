using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreWidget : MonoBehaviour
{
     [SerializeField] private GameObject m_Container;
     [SerializeField] private TextMeshProUGUI m_Text;

     public void SetScore(string text)
     {
          m_Text.text = text;
          m_Container.SetActive(true);
     }
}

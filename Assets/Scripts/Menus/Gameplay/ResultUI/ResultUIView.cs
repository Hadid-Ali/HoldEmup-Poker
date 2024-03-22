using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultUIView : MonoBehaviour
{
    [SerializeField] private ResultHand[] m_ResultHands;
    [SerializeField] private GameObject m_Container;

    [SerializeField] private TextMeshProUGUI m_RowText;
    
    public void SetActiveState(bool status)
    {
        m_Container.SetActive(status);
    }

    public void SetResultData(ResultHandDataObject[] resultHandDataObject,int index)
    {
        m_RowText.text = $"ROW {index + 1} REVEAL";
        for (int i = 0; i < resultHandDataObject.Length; i++)
        {
            m_ResultHands[i].SetDecksContainer(resultHandDataObject[i]);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < m_ResultHands.Length; i++)
        {
            m_ResultHands[i].Reset();
        }
    }
}

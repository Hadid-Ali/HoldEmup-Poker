using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectComponent : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_ContainerObjects;

    private bool m_IsDataSet = false;
    
    public void SetContainerActiveState(bool status)
    {
        gameObject.SetActive(status);
    }
    
    public void EnableAtIndex(int index)
    {
        if (m_IsDataSet)
            return;
        
        m_ContainerObjects[index].SetActive(true);
        m_IsDataSet = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomizedSprite : MonoBehaviour
{
    [SerializeField] private Image m_ImageComponent;
    [SerializeField] private Sprite[] m_SpritesToAssign;
    
    void Start()
    {
        AssignRandomSprite();
    }

    private void AssignRandomSprite()
    {
        m_ImageComponent.sprite = m_SpritesToAssign[Random.Range(0, m_SpritesToAssign.Length)];
    }
}

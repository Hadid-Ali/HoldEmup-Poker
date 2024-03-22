using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts._UXUI
{
    public class AnimatedTextWidget : MonoBehaviour
    {
        [SerializeField] private GameObject m_Container;
        [SerializeField] private TextMeshProUGUI m_LoadingText;
        
        [SerializeField] private int m_TotalLengthToGet;
        [SerializeField] private float m_Wait = 0.25f;

        private float m_NextTypingInstant = 0;
        private string m_DefaultString;

        private int m_Count = 0;
        private bool m_IsActive = false;
        
        public void AnimateText(string text)
        {
            m_DefaultString = text;
            SetActiveState(true);
        }
        
        public void HideText()
        {
            SetActiveState(false);
        }

        private void Update()
        {
            if (!m_IsActive)
                return;
            
            if (Time.time > m_NextTypingInstant)
            {
                m_NextTypingInstant = Time.time + m_Wait;
                AddText();
            }
        }

        private void AddText()
        {
            string textToAssign;
            if (m_Count < m_TotalLengthToGet)
            {
                string text = m_LoadingText.text;
                textToAssign = $"{text}.";
                m_Count++;
            }

            else
            {
                textToAssign = m_DefaultString;
                m_Count = 0;
            }

            SetTextInternal(textToAssign);
        }

        private void SetTextInternal(string text)
        {
            m_LoadingText.text = text;
        }

        private void SetActiveState(bool status)
        {
            m_IsActive = status;
            m_Container.SetActive(status);
        }
    }
}
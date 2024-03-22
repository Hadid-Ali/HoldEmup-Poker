using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ButtonWidget))]
public class ButtonWidget : MonoBehaviour
{
   [SerializeField] private Button m_Button;
   [SerializeField] private ImageWidget m_ButtonImage;

   private void Start()
   {
      Initialize();
   }

   void OnValidate()
   {
      Initialize();
   }

   private void Initialize()
   {
      m_Button ??= GetComponent<Button>();
   }

   public void SubscribeAction(Action action)
   {
      m_Button.onClick.AddListener(() => action());
   }

   public void SetInteractable(bool status)
   {
      m_Button.interactable = status;
   }

   public void SetActiveStatus(bool status)
   {
      gameObject.SetActive(status);
   }

   public void SetButtonImage(Sprite sprite)
   {
      if (m_ButtonImage == null)
         return;
      
      m_ButtonImage.SetImageSprite(sprite);
   }
}

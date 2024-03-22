using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ImageWithLabelWidget : ImageWidget
{
   [SerializeField] private TMP_Text m_Label;

   public override void ResetWidget()
   {
      base.ResetWidget();
      SetLabelText(string.Empty);
   }

   public void SetLabelText(string text)
   {
      if (m_Label == null)
         return;

      SetLabelStatus(!string.IsNullOrEmpty(text));
      m_Label.text = text;
   }

   private void SetLabelStatus(bool status)
   {
      m_Label.gameObject.SetActive(status);
   }
}

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageWidget : MonoBehaviour
{
    [SerializeField] private Image m_ImageComponnet;
    [SerializeField] protected GameObject m_DefaultIcon;
    
    [SerializeField] private Color m_DefaultColor;
    
    public Sprite CurrentSprite => m_ImageComponnet.sprite;
    private Color CurrentColor => m_ImageComponnet.color;
    
    private void Start()
    {
        m_DefaultColor = CurrentColor;
    }
    
    public void SetColor(Color color)
    {
        m_ImageComponnet.color = color;
    }

    public void SetImageSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            m_ImageComponnet.enabled = false;
            return;
        }
        SetDefaultIconStateInternal(false);
        m_ImageComponnet.enabled = true;
        SetImageSpriteInternal(sprite);
    }    
    
    public virtual void ResetWidget()
    {
        SetDefaultIconState();
        SetColor(m_DefaultColor);
    }
    
    public void SetDefaultIconState()
    {
        SetDefaultIconStateInternal(true);
        SetImageSprite(null);
    }
    
    private void SetDefaultIconStateInternal(bool enable)
    {
        if (m_DefaultIcon == null)
            return;
        
        m_DefaultIcon.SetActive(enable);
    } 

    private void SetImageSpriteInternal(Sprite sprite)
    {
        m_ImageComponnet.sprite = sprite;
        SetColor(Color.white);
    }
}
